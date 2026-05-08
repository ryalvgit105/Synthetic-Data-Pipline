using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCameraController : MonoBehaviour
{
    public Transform target;
    public bool randomizeCamera;
    public int randomSeed = 1;

    public int yawSteps = 36;
    public int elevationSteps = 3;
    public int distanceSteps = 3;
    public int randomHoldFrames = 6;
    public float randomMoveLerp = 0.2f;

    public float minPitchDeg = 5f;
    public float maxPitchDeg = 25f;

    public float minDistance = 8f;
    public float maxDistance = 20f;
    public float minTargetScreenFraction = 0.14f;

    Bounds targetBounds;
    bool initialized;
    int frameIndex;
    float currentYaw;
    float currentPitch;
    float currentDistance;
    float targetYaw;
    float targetPitch;
    float targetDistance;
    Camera cachedCamera;

    void Start()
    {
        if (target == null) return;
        targetBounds = ComputeBounds(target);
        initialized = true;
        cachedCamera = GetComponent<Camera>();
        frameIndex = 0;
        currentYaw = 0f;
        currentPitch = Mathf.Lerp(minPitchDeg, maxPitchDeg, 0.5f);
        currentDistance = Mathf.Lerp(minDistance, maxDistance, 0.5f);
        targetYaw = currentYaw;
        targetPitch = currentPitch;
        targetDistance = currentDistance;
    }

    void LateUpdate()
    {
        if (!initialized || target == null) return;

        int frame = frameIndex++;

        float yaw;
        float pitch;
        float distance;

        if (randomizeCamera)
        {
            int hold = Mathf.Max(1, randomHoldFrames);
            if (frame % hold == 0)
            {
                int poseIndex = frame / hold;
                var rng = new System.Random(randomSeed + poseIndex);
                targetYaw = NextRange(rng, 0f, 360f);
                targetPitch = NextRange(rng, minPitchDeg, maxPitchDeg);
                targetDistance = NextRange(rng, minDistance, maxDistance);
            }

            float t = Mathf.Clamp01(randomMoveLerp);
            currentYaw = Mathf.LerpAngle(currentYaw, targetYaw, t);
            currentPitch = Mathf.Lerp(currentPitch, targetPitch, t);
            currentDistance = Mathf.Lerp(currentDistance, targetDistance, t);

            yaw = currentYaw;
            pitch = currentPitch;
            distance = currentDistance;
        }
        else
        {
            int safeYawSteps = Mathf.Max(1, yawSteps);
            int safeElevationSteps = Mathf.Max(1, elevationSteps);
            int safeDistanceSteps = Mathf.Max(1, distanceSteps);

            int yawIndex = frame % safeYawSteps;
            int elevationIndex = (frame / safeYawSteps) % safeElevationSteps;
            int distanceIndex = (frame / (safeYawSteps * safeElevationSteps)) % safeDistanceSteps;

            yaw = (360f * yawIndex) / safeYawSteps;
            float pitchT = safeElevationSteps == 1 ? 0.5f : (float)elevationIndex / (safeElevationSteps - 1);
            float distT = safeDistanceSteps == 1 ? 0.5f : (float)distanceIndex / (safeDistanceSteps - 1);

            pitch = Mathf.Lerp(minPitchDeg, maxPitchDeg, pitchT);
            distance = Mathf.Lerp(minDistance, maxDistance, distT);
        }

        distance = ClampDistanceForTargetSize(distance);

        Vector3 center = targetBounds.center;
        Vector3 dir = Quaternion.Euler(pitch, yaw, 0f) * Vector3.forward;

        transform.position = center - dir * distance;
        transform.LookAt(center);
    }

    static float NextRange(System.Random rng, float min, float max)
    {
        if (max < min)
        {
            (min, max) = (max, min);
        }

        return (float)(min + (max - min) * rng.NextDouble());
    }

    float ClampDistanceForTargetSize(float distance)
    {
        float minFrac = Mathf.Clamp(minTargetScreenFraction, 0.01f, 0.95f);
        if (cachedCamera == null || minFrac <= 0f)
        {
            return distance;
        }

        float fovRad = cachedCamera.fieldOfView * Mathf.Deg2Rad;
        float tanHalf = Mathf.Tan(fovRad * 0.5f);
        if (tanHalf <= 0.0001f)
        {
            return distance;
        }

        float targetHalfHeight = Mathf.Max(targetBounds.extents.y, targetBounds.extents.x * 0.6f);
        float maxDistanceForSize = targetHalfHeight / (minFrac * tanHalf);
        float clamped = Mathf.Min(distance, maxDistanceForSize);

        // Keep the camera outside the target's bounding sphere with extra margin
        // so near views stay close without clipping into the mesh.
        float targetRadius = targetBounds.extents.magnitude;
        float minSafeDistance = targetRadius * 1.2f + cachedCamera.nearClipPlane * 4f;

        return Mathf.Max(Mathf.Max(minDistance, minSafeDistance), clamped);
    }

    static Bounds ComputeBounds(Transform root)
    {
        var renderers = root.GetComponentsInChildren<Renderer>();
        if (renderers.Length == 0) return new Bounds(root.position, Vector3.one);

        Bounds b = renderers[0].bounds;
        for (int i = 1; i < renderers.Length; i++) b.Encapsulate(renderers[i].bounds);
        return b;
    }
}
