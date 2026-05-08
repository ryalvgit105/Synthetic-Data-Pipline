# Unity Synthetic Data Pipeline for Object Detection

## Overview

This repository documents a synthetic data generation pipeline for training and evaluating an object detection model using Unity HDRP, Unity Perception, Google Colab, and You Only Look Once version 8.

The pipeline generates labeled synthetic images of military tanks under controlled visual conditions, converts Unity Perception annotations into You Only Look Once format, trains separate object detection models, and evaluates the trained models on real-world tank imagery.

The purpose of this project is to address data scarcity and labeling limitations in military vehicle detection. Real-world labeled datasets can be limited, expensive to collect, difficult to control, and time-consuming to annotate. This pipeline provides a repeatable and scalable method for generating labeled data automatically and testing how specific visual changes affect model performance.

## Pipeline Summary

High-level workflow:

1. Build a Unity HDRP scene containing a labeled tank, terrain, lighting, and a Perception Camera.
2. Use Unity Perception to automatically capture synthetic images and two-dimensional bounding box labels.
3. Control dataset generation through configurable DatasetProfile assets.
4. Generate separate synthetic datasets under different visual conditions.
5. Export Unity Perception image and JSON annotation files.
6. Move the generated outputs to Google Drive.
7. Use Google Colab to convert Unity Perception JSON labels into You Only Look Once version 8 label format.
8. Train separate You Only Look Once version 8 models for each dataset.
9. Evaluate the trained models on real-world tank imagery.
10. Compare performance using precision, recall, mean average precision, and visual prediction results.

## End-to-End Pipeline Flow

Unity HDRP Scene  
→ Labeled Tank Prefab  
→ DatasetProfile Selected  
→ DatasetRunController Applies Dataset Settings  
→ OrbitCameraController Sets Camera Viewpoints  
→ Lighting, Occlusion, or Tank Color Variation Applied  
→ Unity Perception Captures Images and Bounding Boxes  
→ Image and JSON Files Exported  
→ Google Drive / Google Colab  
→ JSON Labels Converted to You Only Look Once Format  
→ You Only Look Once version 8 Model Training  
→ Real-World Tank Image Evaluation  

## Synthetic Dataset Design

Each dataset was generated independently in Unity and used to train a separate object detection model.

| Dataset | Local Unity Profile | Main Condition Tested | Purpose |
|---|---|---|---|
| Dataset 1 | `DS1_Baseline.asset` | Clean baseline scene | Establishes a controlled baseline for tank detection |
| Dataset 2 | `DS2_Blur.asset` | Lighting and exposure variation | Tests model robustness under changing illumination |
| Dataset 3 | `DS3_Occlusion.asset` | Foreground occlusion | Tests detection when the tank is partially blocked |
| Dataset 4 | `DS4_TankColor.asset` | Tank appearance variation | Tests whether the model can generalize across tank color and material changes |

## Core Unity Scripts

| File | Role in Pipeline |
|---|---|
| `DatasetProfile.cs` | Defines configurable dataset generation parameters such as dataset name, random seed, frame count, camera settings, lighting settings, occlusion settings, and tank appearance settings |
| `DatasetRunController.cs` | Applies the selected dataset profile at runtime and coordinates the camera, lighting, occlusion, tank appearance, and Unity Perception output settings |
| `OrbitCameraController.cs` | Controls camera placement around the tank using grid-based or randomized camera positions |
| `OcclusionSpawner.cs` | Generates foreground occluder objects when occlusion is enabled |
| `PerceptionCamera.prefab` | Contains the Unity Perception Camera, RGB capture, bounding box labeler, and fixed-length capture scenario |
| `LabelConfig_TankOnly.asset` | Defines the single detection class used by the project: `Tank` |

## Google Colab Workflow

The Google Colab notebooks process Unity Perception outputs and train the detection models. The workflow reads `.camera.png` image files and `.frame_data.json` annotation files, converts the Unity Perception bounding boxes into You Only Look Once version 8 `.txt` labels, creates training and validation folder structures, and generates the dataset configuration file needed for model training.

Each synthetic dataset is used to train an independent You Only Look Once version 8 model. The trained models are then evaluated on real-world tank imagery to measure synthetic-to-real transfer.

## Repository Structure

```text
.
├── Colab/
│   ├── ModelComparison/
│   └── ObjectDetectionModelTraining/
│
├── Unity/
│   ├── Prefabs/
│   ├── Scenes/
│   └── Scripts/
│
├── docs/
│   ├── dataset_design.md
│   ├── pipeline_steps.md
│   └── results.md
│
├── unity-perception-setup.md
└── README.md
