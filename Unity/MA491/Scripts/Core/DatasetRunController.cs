using UnityEngine;
using UnityEngine.Perception.GroundTruth.Consumers;
using UnityEngine.Perception.Randomization.Scenarios;
using UnityEngine.Perception.Settings;
using UnityEngine.Rendering;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class DatasetRunController : MonoBehaviour
{
    public DatasetProfile activeProfile;
    public FixedLengthScenario scenario;
    public OrbitCameraController orbitCamera;
    public Light directionalLight;
    // Change this in the DatasetRunController Inspector to control how many iterations the run performs.
    [Header("Scenario Overrides")]
    [Min(1)] public int totalIterations = 500;

    System.Random m_LightRng;
    int m_LightFrame;
    int m_StartFrame;
    Quaternion m_BaseLightRotation;
    Light m_FillDirectionalLight;
    OcclusionSpawner m_OcclusionSpawner;
    int m_LastTankColorBatch = -1;

    void Reset()
    {
        scenario = FindFirstObjectByType<FixedLengthScenario>();
        orbitCamera = FindFirstObjectByType<OrbitCameraController>();
        directionalLight = RenderSettings.sun;
    }

    void Awake()
    {
        if (activeProfile == null)
        {
            Debug.LogWarning("DatasetRunController has no activeProfile assigned.");
            return;
        }

        if (directionalLight != null)
        {
            m_BaseLightRotation = directionalLight.transform.rotation;
        }

        if (activeProfile.enableFillLight)
        {
            var fillGo = new GameObject("RuntimeFillDirectionalLight");
            m_FillDirectionalLight = fillGo.AddComponent<Light>();
            m_FillDirectionalLight.type = LightType.Directional;
            m_FillDirectionalLight.shadows = LightShadows.None;
            m_FillDirectionalLight.color = new Color(1f, 0.98f, 0.95f, 1f);
            m_FillDirectionalLight.intensity = Mathf.Max(0f, activeProfile.fillLightIntensity);
        }

        m_OcclusionSpawner = GetComponent<OcclusionSpawner>();
        if (m_OcclusionSpawner == null)
        {
            m_OcclusionSpawner = gameObject.AddComponent<OcclusionSpawner>();
        }

        m_StartFrame = Time.frameCount;
        ApplyProfile(activeProfile);
    }

    void Update()
    {
#if UNITY_EDITOR
        if (activeProfile != null && activeProfile.autoStopInEditor)
        {
            var elapsedFrames = Time.frameCount - m_StartFrame;
            var targetFrames = Mathf.Max(1, activeProfile.maxRuntimeFrames);

            if (scenario != null)
            {
                // Keep auto-stop in sync with the dataset iteration request.
                var scenarioFrames = scenario.constants.iterationCount * Mathf.Max(1, scenario.framesPerIteration);
                targetFrames = Mathf.Max(targetFrames, scenarioFrames);
            }

            if (elapsedFrames >= targetFrames)
            {
                Debug.Log($"Stopping Play Mode at frame budget: {targetFrames}");
                EditorApplication.isPlaying = false;
            }
        }
#endif

        UpdateTankAppearance();
    }

    void LateUpdate()
    {
        if (activeProfile == null || directionalLight == null || !activeProfile.randomizeDirectionalLight)
        {
            return;
        }

        var frameSeed = activeProfile.seed + 1000003 + m_LightFrame;
        m_LightRng = new System.Random(frameSeed);
        m_LightFrame++;

        if (activeProfile.randomizeLightDirection)
        {
            // Jitter around baseline sun orientation to keep frames visible while adding variation.
            var yawJitter = NextRange(m_LightRng, -25f, 25f);
            var pitchJitter = NextRange(m_LightRng, -10f, 10f);
            directionalLight.transform.rotation = m_BaseLightRotation * Quaternion.Euler(pitchJitter, yawJitter, 0f);
        }
        else
        {
            directionalLight.transform.rotation = m_BaseLightRotation;
        }

        directionalLight.intensity = NextRange(
            m_LightRng,
            activeProfile.lightIntensityRange.x,
            activeProfile.lightIntensityRange.y
        );

        directionalLight.useColorTemperature = true;
        directionalLight.colorTemperature = NextRange(
            m_LightRng,
            activeProfile.lightColorTempRange.x,
            activeProfile.lightColorTempRange.y
        );

        // Keep a minimum ambient term so randomized lighting does not collapse into near-black frames.
        RenderSettings.ambientMode = AmbientMode.Flat;
        RenderSettings.ambientLight = new Color(0.45f, 0.45f, 0.45f, 1f);
        directionalLight.shadowStrength = 0.7f;

        if (m_FillDirectionalLight != null)
        {
            var lookDir = transform.forward;
            if (orbitCamera != null && orbitCamera.target != null)
            {
                lookDir = (orbitCamera.target.position - transform.position).normalized;
            }

            // Directional lights illuminate opposite their forward vector.
            m_FillDirectionalLight.transform.rotation = Quaternion.LookRotation(-lookDir, Vector3.up);
            m_FillDirectionalLight.intensity = Mathf.Max(0f, activeProfile.fillLightIntensity);
        }
    }

    void ApplyProfile(DatasetProfile profile)
    {
        if (scenario != null)
        {
            var framesPerIteration = Mathf.Max(1, profile.framesPerIteration);
            var iterationCount = Mathf.Max(1, totalIterations);

            scenario.framesPerIteration = framesPerIteration;
            scenario.constants.startIteration = 0;
            scenario.constants.iterationCount = iterationCount;
            scenario.constants.randomSeed = (uint)Mathf.Max(0, profile.seed);
        }

        if (orbitCamera != null)
        {
            orbitCamera.randomizeCamera = profile.randomizeCamera;
            orbitCamera.randomSeed = profile.seed;

            orbitCamera.yawSteps = profile.yawSteps;
            orbitCamera.elevationSteps = profile.elevationSteps;
            orbitCamera.distanceSteps = profile.distanceSteps;
            orbitCamera.randomHoldFrames = profile.randomCameraHoldFrames;
            orbitCamera.randomMoveLerp = profile.randomCameraMoveLerp;

            orbitCamera.minPitchDeg = profile.minPitchDeg;
            orbitCamera.maxPitchDeg = profile.maxPitchDeg;
            orbitCamera.minDistance = profile.minDistance;
            orbitCamera.maxDistance = profile.maxDistance;
            orbitCamera.minTargetScreenFraction = profile.minTargetScreenFraction;
        }

        if (m_OcclusionSpawner != null)
        {
            m_OcclusionSpawner.ApplyProfile(profile, orbitCamera);
        }

        ApplyTankAppearance(profile, profile.tankColor);

        var endpoint = PerceptionSettings.endpoint as SoloEndpoint;
        if (endpoint == null)
        {
            endpoint = new SoloEndpoint();
            PerceptionSettings.endpoint = endpoint;
        }

        endpoint.soloDatasetName = profile.datasetName;

        Debug.Log($"Applied dataset profile: {profile.name} ({profile.datasetName}) seed={profile.seed}");
    }

    static float NextRange(System.Random rng, float min, float max)
    {
        if (max < min)
        {
            (min, max) = (max, min);
        }

        return (float)(min + (max - min) * rng.NextDouble());
    }

    void UpdateTankAppearance()
    {
        if (activeProfile == null || orbitCamera == null || orbitCamera.target == null)
        {
            return;
        }

        if (!activeProfile.randomizeTankColor)
        {
            return;
        }

        int elapsed = Mathf.Max(0, Time.frameCount - m_StartFrame);
        int hold = Mathf.Max(1, activeProfile.tankColorHoldFrames);
        int batch = elapsed / hold;
        if (batch == m_LastTankColorBatch)
        {
            return;
        }

        m_LastTankColorBatch = batch;
        var palette = activeProfile.tankColorPalette;
        if (palette == null || palette.Length == 0)
        {
            ApplyTankAppearance(activeProfile, activeProfile.tankColor);
            return;
        }

        var rng = new System.Random(activeProfile.seed + 3000011 + batch);
        var color = palette[rng.Next(palette.Length)];
        ApplyTankAppearance(activeProfile, color);
    }

    void ApplyTankAppearance(DatasetProfile profile, Color color)
    {
        if ((!profile.overrideTankColor && !profile.randomizeTankColor) || orbitCamera == null || orbitCamera.target == null)
        {
            return;
        }

        var renderers = orbitCamera.target.GetComponentsInChildren<Renderer>(true);
        for (int i = 0; i < renderers.Length; i++)
        {
            var renderer = renderers[i];
            if (renderer == null) continue;

            var materials = renderer.materials;
            for (int m = 0; m < materials.Length; m++)
            {
                var mat = materials[m];
                if (mat == null) continue;

                if (mat.HasProperty("_BaseColor")) mat.SetColor("_BaseColor", color);
                if (mat.HasProperty("_Color")) mat.SetColor("_Color", color);
            }
        }
    }
}
