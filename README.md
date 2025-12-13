# Unity Synthetic Data Pipeline for Object Detection

## Overview

This repository documents a complete synthetic data pipeline for training and evaluating an object detection model using **Unity Perception** and **YOLOv8**. The pipeline generates labeled synthetic images of military tanks under progressively more complex visual conditions, trains independent YOLO models on each dataset, and evaluates those models on real-world tank imagery.

Synthetic data enables controlled experimentation that would be difficult, expensive, or unsafe to replicate in the real world. By systematically introducing lighting variation, camera motion, low-visibility conditions, and occlusion, this pipeline isolates how specific environmental factors impact object detection performance and generalization to real images.

The primary problem this pipeline addresses is **data scarcity and controllability** in military vehicle detection. Real labeled datasets are limited and biased; this workflow provides a repeatable, scalable method for generating labeled data and measuring robustness across conditions.

---

## Pipeline Summary

High-level workflow:

- Synthetic data generation in **Unity HDRP** using the **Unity Perception Package**
- Creation of **four synthetic tank datasets**, each introducing additional visual complexity
- Automated bounding box labeling using Perception labelers
- YOLOv8 object detection training in **Google Colab**
- Evaluation of trained models on **real-world tank images** (Roboflow dataset)
- Comparison using bounding boxes, confidence scores, precision, and recall

---

## Synthetic Dataset Design (Datasets 1–4)

Each dataset was generated independently in Unity and trained as a **separate YOLOv8 model**.

### Synthetic Dataset 1 — Baseline

- Static tank and camera  
- Bright daylight lighting  
- No motion blur  
- No occlusion  

**Purpose:** Establish a clean baseline for detection performance.

---

### Synthetic Dataset 2 — Outdoor / Desert Generalization

- Camera pose randomization (angle, height, distance)  
- Directional light randomization (rotation, intensity, color)  
- Desert-style terrain  

**Purpose:** Improve generalization to outdoor daylight scenes.

---

### Synthetic Dataset 3 — Low-Light / Night

- Expanded lighting randomization with low-intensity ranges  
- Reduced ambient lighting  
- HDRP motion blur enabled  

**Purpose:** Improve robustness under poor visibility and motion.

---

### Synthetic Dataset 4 — Extreme Robustness (Multi-Sequence)

- Multiple Perception sequences (separate image/label folders)  
- Aggressive camera motion  
- Heavy motion blur  
- Random foreground occluders  
- Mixed medium-to-dark lighting  

**Purpose:** Stress-test detection under compounded visual degradation.

---

## Repository Structure

```text
.
├── Colab/
│   ├── ModelComparison/              # Scripts and notebooks for comparing trained YOLO models
│   └── ObjectDetectionModelTraining/ # YOLOv8 training, dataset prep, and evaluation notebooks
│
├── Unity/
│   ├── Prefabs/                      # Tank prefab(s) with Labeling component
│   ├── Scenes/                       # Unity Perception scenes for Synthetic Datasets 1–4
│   └── Scripts/                      # Camera, lighting, motion, and occluder randomizers
│
├── docs/
│   ├── dataset_design.md             # Detailed breakdown of Synthetic Datasets 1–4
│   ├── pipeline_steps.md             # End-to-end Unity → Colab → YOLO workflow
│   └── results.md                    # Performance comparison and observations
│
├── unity-perception-setup.md         # Unity Perception installation and configuration guide
└── README.md

