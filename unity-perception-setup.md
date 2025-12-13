# Unity Synthetic Data Pipeline Setup

## Automated Labeling, Randomization
---

## Overview

This repository documents a complete workflow for building an **automated synthetic data pipeline in Unity** for object detection research.
The system is designed to remove manual scene setup and manual annotation by combining:

* **Unity Perception Package** for automatic bounding‑box labeling
* **HDRP (High Definition Render Pipeline)** for physically‑based, high‑fidelity rendering
* **Domain randomization** for robust model training
* **Optional Coplay integration** for experimental LLM‑driven scene control

The primary use case demonstrated in this project is **military vehicle (tank) detection**, with extensibility to other object classes (e.g., drones).

---

## 1. Unity Perception Setup

### Install the Unity Perception Package

The **Unity Perception Package** provides automated ground-truth generation and dataset capture.

**Official Repository:**
[https://github.com/Unity-Technologies/com.unity.perception](https://github.com/Unity-Technologies/com.unity.perception)

#### Installation (Unity Package Manager)

1. Open **Unity**
2. Navigate to:

   ```
   Window → Package Manager
   ```
3. Click the **+** button (top-left)
4. Select **Add package from Git URL**
5. Paste:

   ```
   https://github.com/Unity-Technologies/com.unity.perception.git
   ```
6. Click **Add** and allow Unity to install dependencies

---

### Automatic Labeling and Randomization

The Unity Perception Package is used to:

* Automatically generate **bounding boxes**
* Assign consistent **class labels** (e.g., `tank`)
* Export frame-aligned annotation data during capture

Each target object prefab must include:

* A **Labeling** component
* A consistent label name (e.g., `tank`)

A **Perception Camera** and **Scenario** are added to the scene to:

* Control dataset size
* Manage capture timing
* Apply domain randomization via randomizers

**Reference:** Unity Perception documentation and examples

---

## 2. Unity HDRP Configuration (Required)

Correct HDRP configuration is critical for stable rendering and clean dataset generation.

### A. Fix Render Errors in Unity

1. Open:

   ```
   Edit → Project Settings → Quality
   ```
2. For **each Quality Level** (Low, Medium, High, etc.):

   * Locate the assigned **Render Pipeline Asset**
   * Open it in the Inspector
   * Under **Rendering → Lit Shader Mode**, set:

     * `Both` **or** `Forward Only`
   * Save changes

Repeat for all quality levels listed in Unity warnings.

---

### B. Fix Asynchronous Shader Compilation Errors

1. Open:

   ```
   Edit → Project Settings → Player
   ```
2. Under **Other Settings**, locate **Asynchronous Shader Compilation**
3. **Uncheck** this option

This forces shaders to compile synchronously and prevents:

* GPU hitching
* Frame drops during capture
* Corrupted or incomplete rendered images

---

## 4. Fixing “Purple Materials” (Built‑In → HDRP Conversion)

### Step 1. Confirm HDRP Is Active

1. Open:

   ```
   Edit → Project Settings → Graphics
   ```
2. Ensure **Scriptable Render Pipeline Settings** is assigned to an **HDRP Asset**

If missing:

```
Assets → Create → Rendering → High Definition Render Pipeline Asset
```

Assign it under **Graphics Settings**.

---

### Step 2. Convert Materials Automatically

Use Unity’s built‑in HDRP upgrader:

* Convert all materials:

  ```
  Edit → Render Pipeline → HDRP → Materials → Convert All Built‑in Materials to HDRP
  ```
* Convert specific folders only:

  ```
  Edit → Render Pipeline → HDRP → Materials → Convert Selected Materials to HDRP
  ```

---

### Step 3. Upgrade Lighting and Skybox

1. Run global material upgrade:

   ```
   Edit → Render Pipeline → HDRP → Upgrade Project Materials to HDRP
   ```
2. Open:

   ```
   Window → Rendering → Lighting → Environment
   ```
3. Replace legacy skyboxes with:

   * **HDRI Sky** or
   * **Procedural Sky** (HDRP examples)

---

### Step 4. Check Prefabs and Models

For prefabs with purple meshes:

1. Open the prefab
2. In **Inspector → Mesh Renderer → Materials**:

   * Select any **Standard** materials
3. Click the gear icon → **Select Material in Project**
4. Right‑click the material → **Convert to HDRP Material**

---

### Step 5. Fix Textures Manually (If Needed)

If automatic conversion fails:

1. Open the affected material
2. Set **Shader → HDRP/Lit**
3. Reassign textures:

   * **Base Map** → Albedo
   * **Mask Map** → Metallic / Smoothness / Occlusion (combined)
   * **Normal Map** → Normal
   * **Emission Map** → Emission

---

### Step 6. Re‑import Assets

* Right‑click the asset folder → **Reimport**
* Re‑run HDRP material conversion if new materials appear

---

### Result

All imported assets render correctly under HDRP with:

* No purple materials
* Physically accurate lighting
* Stable rendering during synthetic data capture

---

## Status

Active development — **MA489 Synthetic Computer Vision Research**

