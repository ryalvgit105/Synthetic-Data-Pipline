## Synthetic Datasets

Due to size constraints, synthetic datasets are not stored directly in this repository.

Datasets were generated in Unity and stored externally in Google Drive:

- syntheticDataset1
- syntheticDataset2
- syntheticDataset3
- syntheticDataset4

Each dataset contains:
- Images/
- Labels/ (YOLO format)

All datasets were used to independently train YOLOv8 models.

Synthetic Tank Dataset Descriptions:
Dataset 1 — Baseline / Silhouette

Purpose: Establish a clean baseline for learning tank shape.

Tank appearance: Black / silhouette

Background: Simple, uncluttered

Lighting: Bright, static

Camera motion: None

Motion blur: None

Occluders: None

Intent:
Ensure the model learns the basic geometry of a tank without environmental noise.



Dataset 2 — Environmental Realism

Purpose: Introduce realistic scene context.

Tank appearance: Realistic color

Background: Military base environment

Lighting: Outdoor / daylight

Camera motion: Present

Motion blur: Moderate

Occluders: Minimal

Intent:
Improve generalization by exposing the model to realistic environments and mild motion effects.

Dataset 3 — Feature Emphasis

Purpose: Force the model to focus on tank features rather than background.

Tank appearance: Color/highlighted to emphasize features

Background: Black-and-white or simplified

Lighting: Controlled

Camera motion: None

Motion blur: None

Occluders: None

Intent:
Encourage learning of edges, contours, and distinguishing tank features.

Dataset 4 — Stress Test / Hard Cases

Purpose: Test robustness under extreme conditions.

Tank appearance: Varied

Background: Cluttered

Camera motion: Aggressive

Motion blur: High

Occluders: Random objects

Structure: Multiple sequences

Intent:
Expose the model to worst-case scenarios and identify failure modes, even at the cost of precision.
