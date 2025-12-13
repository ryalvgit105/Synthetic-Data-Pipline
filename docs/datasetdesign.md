# dataset_design.md

## Synthetic Dataset Design (Datasets 1–4)

This document describes the design and purpose of the four synthetic tank datasets generated using Unity Perception. Each dataset was created independently and trained as a separate YOLOv8 object detection model to isolate how specific visual conditions affect detection performance and robustness.

All datasets use the same base tank prefab, Perception Camera, and bounding box labeler. Differences between datasets are introduced through Unity randomization scripts and rendering settings.

---

## Common Configuration (All Datasets)

### Unity Components
- **Perception Camera**
  - BoundingBox2DLabeler
  - Label: `tank`
- **Scenario**
  - FixedLengthScenario
- **Tank Prefab**
  - Labeling component applied to root object
- **Output**
  - RGB images
  - Unity Perception JSON annotations

### Detection Task
- Single-class object detection
- Class: `tank`

---

## Synthetic Dataset 1 — Baseline
<img width="733" height="314" alt="image" src="https://github.com/user-attachments/assets/7001113e-f032-461f-a63f-9566c401871e" />


### Purpose
Establish a clean baseline for tank detection with minimal visual complexity.

### Configuration
- Static tank
- Static camera
- Bright daylight lighting
- No motion blur
- No occlusion
- No camera or lighting randomization

### Scripts Used
- Perception Camera
- FixedLengthScenario
- BoundingBox2DLabeler

### Expected Impact
- High confidence detections
- Clean bounding boxes
- Strong baseline precision
- Limited robustness to real-world variation

---

## Synthetic Dataset 2 — Outdoor / Desert Generalization
<img width="733" height="314" alt="image" src="https://github.com/user-attachments/assets/9800ef5c-a216-4a27-8142-3514d4abd677" />

### Purpose
Improve generalization to outdoor daylight environments.

### Added Variations
- Camera pose randomization (angle, height, distance)
- Directional light randomization (rotation, intensity, color)
- Desert-style terrain

### Scripts Used
- Camera randomizer
- Light randomizer
- Perception Camera and labelers

### Expected Impact
- Increased viewpoint diversity
- Slightly noisier labels
- Improved performance on real daylight images

---

## Synthetic Dataset 3 — Low-Light / Night
<img width="733" height="314" alt="image" src="https://github.com/user-attachments/assets/09afd681-94f0-4af4-a7f0-d8a0a66fa507" />

### Purpose
Improve robustness under poor visibility and motion.

### Added Variations
- Expanded lighting randomization with low-intensity ranges
- Reduced ambient lighting
- HDRP motion blur enabled
- Increased camera movement ranges

### Scripts Used
- Camera randomizer (expanded range)
- Light randomizer (low-light configuration)
- HDRP Motion Blur volume override

### Expected Impact
- Lower confidence scores
- Increased training time
- Improved detection in dark or motion-heavy scenes

---

## Synthetic Dataset 4 — Extreme Robustness (Multi-Sequence)
<img width="732" height="314" alt="image" src="https://github.com/user-attachments/assets/1b60a976-94ba-4c37-89cb-239e3a5fd5b1" />

### Purpose
Stress-test object detection under compounded visual degradation.

### Structural Difference
- Multiple Perception sequences
- Each sequence contains its own image and label folders

### Added Variations
- Aggressive camera motion
- Heavy motion blur
- Random foreground occluders
- Mixed medium-to-dark lighting conditions
- High clutter and partial occlusion

### Scripts Used
- Occluder randomizer
- Camera randomizer (aggressive)
- Light randomizer (wide range)
- HDRP Motion Blur (high intensity)

### Expected Impact
- Lowest raw precision
- Noisy bounding boxes
- Strong robustness learning
- Risk of degrading clean-scene performance

---

## Summary

Each dataset incrementally introduces new visual challenges, allowing controlled analysis of how environmental factors affect object detection performance and synthetic-to-real transfer.
