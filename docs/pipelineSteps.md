# pipeline_steps.md

## End-to-End Pipeline Overview

This document outlines the complete workflow used to generate synthetic data in Unity, train YOLOv8 object detection models in Google Colab, and evaluate those models on real-world tank images.

---

## Step 1: Unity Environment Setup

1. Create a Unity HDRP project
2. Install the Unity Perception Package
3. Import or create a tank prefab
4. Add a Labeling component to the tank root
   - Label: `tank`
5. Add a Perception Camera
   - Enable BoundingBox2DLabeler
6. Create a FixedLengthScenario

This setup provides automatic bounding box annotation for all rendered frames.

---

## Step 2: Synthetic Dataset Generation

1. Configure Unity scenes for each dataset (1–4)
2. Apply dataset-specific randomization scripts:
   - Camera randomizers
   - Lighting randomizers
   - Motion blur (HDRP)
   - Occluder placement (Dataset 4)
3. Run the Perception Scenario
4. Export RGB images and JSON annotation files

Each dataset is generated independently to ensure clean experimental separation.

---

## Step 3: Dataset Preparation in Google Colab

1. Mount Google Drive
2. Load Unity Perception JSON annotation files
3. Convert JSON annotations to YOLO format
   - Normalize bounding box coordinates
   - Single-class mapping (`tank`)
4. Split data into training and validation sets
5. Generate YOLO dataset YAML file

This step standardizes all datasets for YOLOv8 training.

---

## Step 4: YOLOv8 Model Training

1. Install Ultralytics YOLO in Colab
2. Load base YOLOv8 model weights
3. Train a separate model for each synthetic dataset
4. Save trained weights to Google Drive

All models are trained independently to enable direct comparison.

---

## Step 5: Real-World Evaluation

1. Load real-world tank images (Roboflow dataset)
2. Run inference using each trained YOLO model
3. Visualize detections and confidence scores
4. Compare bounding boxes, precision, and recall across models

This step evaluates synthetic-to-real transfer performance.

---

## Step 6: Analysis and Comparison

1. Compare performance trends across datasets
2. Identify strengths and weaknesses of each synthetic configuration
3. Document results in `results.md`

---

## Outcome

This pipeline enables repeatable, controlled experimentation on how synthetic data complexity affects object detection performance and generalization to real-world imagery.
