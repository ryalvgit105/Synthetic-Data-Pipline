# Results and Analysis

This document summarizes the performance of YOLOv8 object detection models trained on four independently generated synthetic tank datasets. Each model was evaluated on the same set of real-world tank images to assess synthetic-to-real generalization.

Evaluation focuses on **precision** and **recall**, measured consistently across all pipelines.

---

## Evaluation Setup

- **Task:** Single-class object detection (`tank`)
- **Models:** YOLOv8 (one model per synthetic dataset)
- **Training Data:** Unity-generated synthetic datasets 1–4
- **Evaluation Data:** Real-world tank images (Roboflow)
- **Metrics Reported:**
  - Precision
  - Recall
- **Inference Conditions:** Identical confidence thresholds and evaluation scripts for all models

---

## Precision Comparison

<img width="745" height="648" alt="image" src="https://github.com/user-attachments/assets/5e13b638-448d-4127-b9aa-45cc6e99cdc4" />


### Observations

- **Pipeline 1 (Dataset 1 – Baseline)**  
  Lowest precision. The lack of variability caused overfitting to clean synthetic conditions, leading to false positives on real imagery.

- **Pipeline 2 (Dataset 2 – Desert / Outdoor)**  
  Significant precision improvement due to camera pose and lighting randomization.

- **Pipeline 3 (Dataset 3 – Low-Light / Motion)**  
  Highest precision overall. Exposure to challenging lighting and motion conditions improved the model’s ability to reject false detections.

- **Pipeline 4 (Dataset 4 – Extreme Robustness)**  
  Precision decreased compared to Pipeline 3, indicating that excessive degradation and occlusion may reduce clean-scene discrimination.

---

## Recall Comparison

<img width="751" height="653" alt="image" src="https://github.com/user-attachments/assets/6f0dc6f2-b7f2-4a59-922a-952c548f1f13" />


### Observations

- **Pipeline 1**  
  Lowest recall, missing many real-world tanks due to limited training diversity.

- **Pipeline 2**  
  Improved recall, reflecting better viewpoint generalization.

- **Pipeline 3**  
  Highest recall across all pipelines. Training under low-light and motion conditions increased the model’s ability to detect tanks under varied real-world conditions.

- **Pipeline 4**  
  Recall dropped relative to Pipeline 3, suggesting that extreme occlusion and clutter suppressed detection confidence.

---

## Cross-Pipeline Comparison Summary

| Pipeline | Dataset Focus | Precision Trend | Recall Trend |
|--------|---------------|-----------------|--------------|
| Pipeline 1 | Clean baseline | Lowest | Lowest |
| Pipeline 2 | Outdoor generalization | Improved | Improved |
| Pipeline 3 | Low-light + motion | Highest | Highest |
| Pipeline 4 | Extreme degradation | Moderate | Reduced |

---

## Key Takeaways

- Incremental realism improves synthetic-to-real transfer.
- Moderate difficulty (Dataset 3) produced the best balance of precision and recall.
- Excessive degradation (Dataset 4) introduces diminishing returns and performance regression.
- Synthetic dataset design should prioritize **controlled realism**, not maximal visual stress.

---

## Implications for Future Work

- Dataset 5 should combine:
  - Dataset 3 lighting and motion
  - Limited, probabilistic occlusion
  - Reduced extreme clutter
- Adaptive curriculum training may further improve robustness.
- These results support synthetic data as a viable alternative when real labeled data is scarce.

---

## Course Context

This evaluation was conducted as part of **MA489**, supporting research into synthetic data pipelines, object detection robustness, and real-world generalization for military vehicle detection.
