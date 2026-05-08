using UnityEngine;

[CreateAssetMenu(fileName = "DatasetProfile", menuName = "MA491/Dataset Profile")]
public class DatasetProfile : ScriptableObject
{
    [Header("Output")]
    public string datasetName;
    public int seed = 539662031;

    [Header("Scenario")]
    public int framesPerIteration = 324;
    public int totalIterations = 100;
    public bool autoStopInEditor = true;
    public int maxRuntimeFrames = 160;

    [Header("Camera")]
    public bool randomizeCamera = false;
    public int yawSteps = 36;
    public int elevationSteps = 3;
    public int distanceSteps = 9;
    public int randomCameraHoldFrames = 6;
    public float randomCameraMoveLerp = 0.2f;
    public float minPitchDeg = 5f;
    public float maxPitchDeg = 25f;
    public float minDistance = 4f;
    public float maxDistance = 300f;
    [Range(0.02f, 0.5f)] public float minTargetScreenFraction = 0.03f;

    [Header("Lighting")]
    public bool randomizeDirectionalLight = false;
    public bool randomizeLightDirection = true;
    public Vector2 lightIntensityRange = new Vector2(1.0f, 1.0f);
    public Vector2 lightColorTempRange = new Vector2(6500f, 6500f);
    public bool enableFillLight = false;
    public float fillLightIntensity = 1.2f;

    [Header("Occlusion")]
    public bool enableOcclusion = false;
    [Range(0f, 1f)] public float occlusionFrameProbability = 0.4f;
    public int occluderCount = 2;
    public int occluderHoldFrames = 8;
    [Range(0.05f, 0.95f)] public float occluderDistanceMin01 = 0.25f;
    [Range(0.05f, 0.95f)] public float occluderDistanceMax01 = 0.65f;
    public float occluderScaleMin = 0.8f;
    public float occluderScaleMax = 1.8f;
    public float occluderLateralSpread = 1.2f;
    public bool useVariedOccluderShapes = true;
    public bool useProjectOccluderPrefabs = false;
    public string[] projectOccluderPrefabPaths = new string[0];

    [Header("Appearance")]
    public bool overrideTankColor = false;
    public Color tankColor = new Color(0.72f, 0.67f, 0.52f, 1f);
    public bool randomizeTankColor = false;
    public int tankColorHoldFrames = 10;
    public Color[] tankColorPalette = new[]
    {
        new Color(0.74f, 0.70f, 0.54f, 1f), // sand
        new Color(0.33f, 0.38f, 0.24f, 1f), // olive
        new Color(0.28f, 0.28f, 0.28f, 1f), // gray
        new Color(0.18f, 0.18f, 0.18f, 1f)  // dark
    };
}

