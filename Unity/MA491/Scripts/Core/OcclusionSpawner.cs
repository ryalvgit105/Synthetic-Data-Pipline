using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class OcclusionSpawner : MonoBehaviour
{
    public OrbitCameraController orbitCamera;
    public int seed = 1;
    public bool enableOcclusion;
    public float occlusionFrameProbability = 0.4f;
    public int occluderCount = 2;
    public int occluderHoldFrames = 8;
    public float occluderDistanceMin01 = 0.25f;
    public float occluderDistanceMax01 = 0.65f;
    public float occluderScaleMin = 0.8f;
    public float occluderScaleMax = 1.8f;
    public float occluderLateralSpread = 1.2f;
    public bool useVariedOccluderShapes = true;
    public bool useProjectOccluderPrefabs = false;
    public string[] projectOccluderPrefabPaths = new string[0];

    readonly List<GameObject> m_Occluders = new List<GameObject>();
    int m_Frame;

    public void ApplyProfile(DatasetProfile profile, OrbitCameraController cameraController)
    {
        orbitCamera = cameraController;
        seed = profile.seed + 2000003;
        enableOcclusion = profile.enableOcclusion;
        occlusionFrameProbability = Mathf.Clamp01(profile.occlusionFrameProbability);
        occluderCount = Mathf.Max(0, profile.occluderCount);
        occluderHoldFrames = Mathf.Max(1, profile.occluderHoldFrames);
        occluderDistanceMin01 = Mathf.Clamp(profile.occluderDistanceMin01, 0.05f, 0.95f);
        occluderDistanceMax01 = Mathf.Clamp(profile.occluderDistanceMax01, 0.05f, 0.95f);
        occluderScaleMin = Mathf.Max(0.05f, profile.occluderScaleMin);
        occluderScaleMax = Mathf.Max(occluderScaleMin, profile.occluderScaleMax);
        occluderLateralSpread = Mathf.Max(0f, profile.occluderLateralSpread);
        useVariedOccluderShapes = profile.useVariedOccluderShapes;
        useProjectOccluderPrefabs = profile.useProjectOccluderPrefabs;
        projectOccluderPrefabPaths = profile.projectOccluderPrefabPaths;

        RebuildPool();
    }

    void RebuildPool()
    {
        foreach (var go in m_Occluders)
        {
            if (go != null)
            {
                Destroy(go);
            }
        }

        m_Occluders.Clear();

        if (!enableOcclusion || occluderCount <= 0)
        {
            return;
        }

        for (int i = 0; i < occluderCount; i++)
        {
            var go = CreateOccluderObject(i);
            go.name = $"RuntimeOccluder_{i}";
            go.transform.SetParent(transform, false);

            var colliders = go.GetComponentsInChildren<Collider>(true);
            for (int c = 0; c < colliders.Length; c++)
            {
                Destroy(colliders[c]);
            }

            var renderers = go.GetComponentsInChildren<Renderer>(true);
            for (int r = 0; r < renderers.Length; r++)
            {
                var renderer = renderers[r];
                if (renderer == null) continue;
                renderer.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
                renderer.receiveShadows = false;
                var mat = renderer.material;
                if (mat != null)
                {
                    var baseColor = new Color(0.24f, 0.24f, 0.24f, 1f);
                    mat.color = baseColor;
                    if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", baseColor);
                    if (mat.HasProperty("_Metallic")) mat.SetFloat("_Metallic", 0f);
                    if (mat.HasProperty("_Smoothness")) mat.SetFloat("_Smoothness", 0.05f);
                }
            }

            go.SetActive(false);
            m_Occluders.Add(go);
        }
    }

    GameObject CreateOccluderObject(int index)
    {
        var rng = new System.Random(seed + 911 + index);
        var prefab = TryLoadOccluderPrefab(rng);
        if (prefab != null)
        {
            return Instantiate(prefab);
        }

        var primitive = PrimitiveType.Cube;
        if (useVariedOccluderShapes)
        {
            var choices = new[] { PrimitiveType.Cube, PrimitiveType.Sphere, PrimitiveType.Capsule, PrimitiveType.Cylinder };
            primitive = choices[rng.Next(choices.Length)];
        }

        return GameObject.CreatePrimitive(primitive);
    }

    GameObject TryLoadOccluderPrefab(System.Random rng)
    {
        if (!useProjectOccluderPrefabs || projectOccluderPrefabPaths == null || projectOccluderPrefabPaths.Length == 0)
        {
            return null;
        }

#if UNITY_EDITOR
        var validPrefabs = new List<GameObject>();
        for (int i = 0; i < projectOccluderPrefabPaths.Length; i++)
        {
            var path = projectOccluderPrefabPaths[i];
            if (string.IsNullOrWhiteSpace(path)) continue;
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab != null) validPrefabs.Add(prefab);
        }

        if (validPrefabs.Count == 0)
        {
            return null;
        }

        return validPrefabs[rng.Next(validPrefabs.Count)];
#else
        return null;
#endif
    }

    void LateUpdate()
    {
        if (!enableOcclusion || orbitCamera == null || orbitCamera.target == null || m_Occluders.Count == 0)
        {
            SetAllActive(false);
            return;
        }

        int hold = Mathf.Max(1, occluderHoldFrames);
        if (m_Frame % hold == 0)
        {
            int batch = m_Frame / hold;
            PositionOccluders(batch);
        }

        m_Frame++;
    }

    void PositionOccluders(int batch)
    {
        var rng = new System.Random(seed + batch);

        bool show = rng.NextDouble() < occlusionFrameProbability;
        if (!show)
        {
            SetAllActive(false);
            return;
        }

        Vector3 camPos = transform.position;
        Vector3 targetPos = orbitCamera.target.position;
        Vector3 toTarget = targetPos - camPos;
        float targetDistance = Mathf.Max(0.1f, toTarget.magnitude);

        Vector3 forward = toTarget.normalized;
        Vector3 right = Vector3.Cross(Vector3.up, forward).normalized;
        if (right.sqrMagnitude < 0.001f) right = Vector3.right;
        Vector3 up = Vector3.Cross(forward, right).normalized;

        float spreadScale = occluderLateralSpread;
        float nearTargetDistMin = targetDistance * Mathf.Clamp(occluderDistanceMin01, 0.05f, 0.95f);
        float nearTargetDistMax = targetDistance * Mathf.Clamp(occluderDistanceMax01, 0.05f, 0.95f);

        for (int i = 0; i < m_Occluders.Count; i++)
        {
            var go = m_Occluders[i];
            if (go == null) continue;

            // Place occluders around the tank with a camera-facing bias.
            // 1) Start from a point in front of the tank (toward the camera), then
            // 2) Scatter around that point using an azimuth + vertical offset.
            float frontDist = NextRange(rng, nearTargetDistMin, nearTargetDistMax);
            Vector3 frontBase = targetPos - forward * frontDist;

            float azimuthDeg = NextRange(rng, 0f, 360f);
            float azimuthRad = azimuthDeg * Mathf.Deg2Rad;
            float radial = NextRange(rng, 0f, spreadScale);
            float vertical = NextRange(rng, -spreadScale * 0.7f, spreadScale * 0.7f);

            Vector3 ringOffset = right * (Mathf.Cos(azimuthRad) * radial) + forward * (Mathf.Sin(azimuthRad) * radial);
            Vector3 pos = frontBase + ringOffset + up * vertical;
            go.transform.position = pos;

            float sx = NextRange(rng, occluderScaleMin, occluderScaleMax);
            float sy = NextRange(rng, occluderScaleMin, occluderScaleMax);
            float sz = NextRange(rng, occluderScaleMin * 0.4f, occluderScaleMax * 0.8f);
            go.transform.localScale = new Vector3(sx, sy, sz);

            float yaw = NextRange(rng, 0f, 360f);
            float pitch = NextRange(rng, -25f, 25f);
            go.transform.rotation = Quaternion.Euler(pitch, yaw, 0f);

            go.SetActive(true);
        }
    }

    void SetAllActive(bool isActive)
    {
        for (int i = 0; i < m_Occluders.Count; i++)
        {
            var go = m_Occluders[i];
            if (go != null && go.activeSelf != isActive)
            {
                go.SetActive(isActive);
            }
        }
    }

    static float NextRange(System.Random rng, float min, float max)
    {
        if (max < min)
        {
            (min, max) = (max, min);
        }

        return (float)(min + (max - min) * rng.NextDouble());
    }
}
