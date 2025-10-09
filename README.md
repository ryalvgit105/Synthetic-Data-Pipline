# Synthetic-Data-Pipline
Synthetic Data generation pipeline using NVIDIA OMNIVERSE
# Omniverse Kit App — Project README

> GPU‑accelerated, OpenUSD‑based application scaffold using NVIDIA Omniverse Kit SDK. Includes a simple 2D→3D asset pipeline and repeatable launch scripts.

---

## Table of Contents

* [Overview](#overview)
* [Prerequisites](#prerequisites)
* [Repository Structure](#repository-structure)
* [Quick Start (Windows)](#quick-start-windows)
* [Launching Again](#launching-again)
* [Create a Desktop Shortcut](#create-a-desktop-shortcut)
* [2D→3D Asset Pipeline (Hunyuan 3D & NeRF)](#2d3d-asset-pipeline-hunyuan-3d--nerf)
* [Importing .OBJ into Omniverse (Asset Importer)](#importing-obj-into-omniverse-asset-importer)
* [Troubleshooting](#troubleshooting)
* [Useful Links](#useful-links)
* [License & Contributions](#license--contributions)

---

## Overview

This repo is a working project scaffold based on the NVIDIA **Omniverse Kit SDK** template. It pairs a clean Kit app setup with a lightweight **2D→3D model** workflow so you can quickly generate assets and load them into a Kit‑based viewer/editor.

**Why this exists**

* Start from a known‑good Kit app template
* Re‑launch the app quickly (no hunting for commands)
* Convert 2D images into 3D assets using free daily tokens
* Import **.OBJ** assets cleanly into **USD** for Omniverse scenes

If you’re prototyping, a Feature Branch of Kit SDK is fine. For production, prefer the **Production Branch** via **NGC**.

---

## Prerequisites

* **OS:** Windows 10/11 (or Linux; instructions below are Windows‑focused)
* **GPU:** NVIDIA RTX (3070 or better recommended)
* **Driver:** 537.58+ (newer may work but may be less tested)
* **Tools:**

  * Git & **Git LFS**
  * (Windows, C++ only) **Visual Studio 2019/2022** with *Desktop development with C++* + **Windows SDK**
  * (Linux) `build-essential`
* **Optional (Linux):** Docker + NVIDIA Container Toolkit
* **Editor:** VS Code (or preferred IDE)

> **Folder used in this project:** `C:\Users\User\Desktop\omniverse\kit-app-template`

---

## Repository Structure

```
.vscode/           # VS Code tasks & helpers
readme-assets/     # Images & docs for the README
templates/         # Kit templates (apps & extensions)
tools/             # Local repo tooling configs
.editorconfig
.gitattributes
.gitignore
LICENSE
README.md          # This file
premake5.lua       # Build config (what apps to build)
repo.bat           # Windows repo entry point
repo.sh            # Linux repo entry point
repo.toml          # Top-level repo tools config
repo_tools.toml    # Local repo tools setup
```

---

## Quick Start (Windows)

1. **Clone** the Kit template (if you haven’t already):

```bat
cd "C:\Users\User\Desktop\omniverse"
git clone https://github.com/NVIDIA-Omniverse/kit-app-template.git
cd kit-app-template
```

2. **Create a new app** from the template:

```bat
.\nrepo.bat template new
```

Select:

* **Create:** `Application`
* **Template:** `Kit Base Editor`
* **.kit file name:** lowercase, alphanumeric (e.g., `myapp`)
* **Display name:** human‑readable (e.g., `My App`)
* **Version:** semantic versioning recommended (e.g., `0.1.0`)
* **Application layers:** `No` (add later if you need streaming)

3. **Build** your app:

```bat
.
repo.bat build
```

Look for: `BUILD (RELEASE) SUCCEEDED`.

4. **Launch** your app:

```bat
.
repo.bat launch
```

Choose your app from the menu. First launch may take **5–8 minutes** for initial shader compilation; it’s much faster after that.

---

## Launching Again

If you already built once and just want to run it again:

```bat
cd "C:\Users\User\Desktop\omniverse\kit-app-template"
.
repo.bat launch
```

**Direct executable (skip the menu):**

```
C:\Users\User\Desktop\omniverse\kit-app-template\build\Release\apps\<your_app>\<your_app>.exe
```

Run that `.exe` directly.

> **Common error:** `'.\repo.bat' is not recognized…` ⇒ you’re likely not in the folder that actually contains `repo.bat`. `cd` into `kit-app-template` first.

---

## Create a Desktop Shortcut

Create a `launch_omniverse.bat` so you can double‑click to start your app.

**Steps**

1. Open **Notepad** and paste:

```bat
@echo off
cd /d "C:\Users\User\Desktop\omniverse\kit-app-template"
.
repo.bat launch
pause
```

2. Save as **`launch_omniverse.bat`** (Save as type: *All Files*), e.g., on your **Desktop**.
3. Double‑click to launch. Keep `pause` if you want the window to remain open for logs.

---

## 2D→3D Asset Pipeline (Hunyuan 3D & NeRF)

This project documents two simple paths for generating 3D assets from photos:

### Option A — Tencent **Hunyuan 3D** (recommended for quick results)

* Converts **2D images → 3D models**; offers **~20 free tokens/day** (handy for experimentation)
* Tested with a personal photo; **worked well** in practice
* Export a standard **`.obj`** (with textures if available) for import to Omniverse

**Pros:** Fast, minimal setup, good results.
**Cons:** Daily token limits; online service dependency.

### Option B — **NeRF** (NVIDIA / other NeRF tools)

* Reconstructs 3D from a **set of images**; great for scenes/objects with multiple viewpoints
* Results can be more variable and require more cleanup than Hunyuan 3D in this workflow

**Pros:** Powerful for real‑world captures.
**Cons:** Heavier compute; more steps; output quality depends on capture.

> For both options, the target for Omniverse import in this project is **`.obj`**.

---

## Importing .OBJ into Omniverse (Asset Importer)

The easiest path is via the **Asset Importer** inside **USD Composer** (formerly Omniverse Create). This converts your `.obj` into **USD**, Omniverse’s native format.

**Steps**

1. Open **USD Composer** (or any Omniverse app that includes Asset Importer).
2. In the **Content Browser**, browse to the folder containing your `.obj`.
3. **Right‑click** in the Content Browser → **Import and Convert**.
4. Select your **`.obj`** file.
5. In options, consider enabling:

   * **Materials** (uses `.mtl` + textures if available)
   * **UsdPreviewSurface** (simpler material path; often more compatible)
   * **Smooth Normals**
   * **Meter as World Unit** (if your asset scale expects meters)
6. Set the **Convert to** target folder (where the new `.usd` will be saved).
7. Click **Convert** / **Import**.
8. Drag the generated **USD** from the Content Browser into your **stage/viewport**.

**Video reference:** *USD Composer OBJ Import (YouTube)* — e.g., [https://www.youtube.com/watch?v=dQI0OpzfVHw](https://www.youtube.com/watch?v=dQI0OpzfVHw)

---

## Next Steps — Synthetic Data & Scene Creation

### 3. Synthetic Data Generation / Scene Creation

#### A) Scene Creation with **OpenUSD**

* **Why OpenUSD**: Non‑destructive, layered scene graphs; easy to kitbash environments and swap assets/variants.
* **Core concepts**: *Stage* (your scene), *Layering* (composition arcs), *Variants* (style/LOD/state), *Payloads/References* (bring large assets on demand).
* **Quick win**: Start a new **empty Stage** in USD Composer, set **metersPerUnit = 1.0**, and create folders:

  * `/Scenes` (top‑level USD stages)
  * `/Assets` (USD assets: buildings, props, characters)
  * `/Materials` (MDL/PreviewSurface)
  * `/Textures` (maps)
* **Connect any data source**: Import CAD/OBJ/GLTF/FBX → convert to USD once → reuse everywhere.

> Resource: Universal Scene Description (OpenUSD) — “Connecting Any Data Source” (intro & best practices).

#### B) Data Generation & Randomization with **Isaac Sim Replicator**

* **Install Isaac Sim** (see below), enable **Replicator** and **Replicator Composer** extensions.
* **Pipeline**:

  1. **Author scene** (USD): lights, cameras, assets.
  2. **Randomize**: positions, rotations, scales, materials, lights, HDRIs, clutter.
  3. **Annotate**: RGB, depth, instance/semantic seg, 2D/3D bboxes, normals.
  4. **Export**: COCO/YOLO or custom JSON + images to `./datasets/<run-id>`.
* **GUI path (Composer)**: *Window → Isaac Sim → Replicator Composer* → add randomizers → choose writers (COCO/TFRecord/YOLO) → **Run**.
* **Scripting path (Kit/Isaac)**: minimal Python sketch

  ```python
  import omni.replicator.core as rep
  with rep.new_layer():
      cam = rep.create.camera(); light = rep.create.light(intensity=5000)
      meshes = rep.create.from_usd("/Assets/props/*.usd")
      with rep.trigger.on_frame(num_frames=500):
          with meshes:
              rep.modify.pose(position=rep.distribution.uniform((-1,0,-1),(1,0.5,1)),
                               rotation=rep.distribution.uniform((0,0,0),(0,360,0)))
          rep.writer.attach(rep.WriterRegistry.get("BasicWriter"),
                            output_dir="./datasets/run1",
                            rgb=True, bbox_2d_tight=True, semantic_segmentation=True)
  ```

#### C) **Isaac Sim** (What/Why)

* **What**: High‑fidelity robotics simulator on Omniverse (PhysX + RTX rendering + AI data tools).
* **Why**: One environment for physics, sensors (camera/LiDAR), ROS2, and **mass synthetic data**.
* **Use cases**: AMR/navigation, manipulation, perception dataset generation, digital twins.

#### D) Install **Isaac Sim 4.5 / Isaac Lab 2.0**

* Follow the official installer for your OS/GPU drivers.
* After install: launch Isaac Sim → *Extension Manager* → enable **Replicator**, **Replicator Composer**, **ROS/ROS2** (as needed).
* Test: *Window → Isaac Sim → Replicator Composer* → load a sample scene → run a small 50‑frame export.

#### E) Creating Your Own **Omniverse Extension**

* Goal: Package tools (UI panels, operators, randomizers) as reusable extensions.
* **Scaffold**: `repo.bat template new` → choose **Extension (Python UI)** → name `com.yourorg.randomizer`.
* **Add UI**: Use `omni.ui` for panels; bind buttons to Replicator randomizers.
* **Register**: Add your extension folder to `exts/` and ensure it’s listed in your app’s `.kit` file (under `[settings.app.extensions]`).
* **Ship**: Version via semver; include `config/extension.toml`, README, and sample USD.

---

## Troubleshooting

* **repo.bat not found**

  * Ensure you’re in: `C:\Users\User\Desktop\omniverse\kit-app-template`
  * Run: `dir` and confirm `repo.bat` is listed

* **Build succeeded but nothing launches**

  * Use `repo.bat launch` and select your app, or run the built `.exe` directly from `build\Release\apps\<your_app>`

* **Slow first launch**

  * First start compiles shaders (**5–8 min**). Subsequent launches are much faster.

* **OBJ imports with no textures**

  * Confirm your `.mtl` references correct texture file names/paths
  * Keep textures in the same folder as the `.obj` when importing
  * Try **UsdPreviewSurface** if MDL gives unexpected results

---

## Useful Links

* **Kit App Template (GitHub):** [https://github.com/NVIDIA-Omniverse/kit-app-template](https://github.com/NVIDIA-Omniverse/kit-app-template)
* **NVIDIA Omniverse (general):** [https://www.nvidia.com/omniverse/](https://www.nvidia.com/omniverse/)
* **OBJ → USD import (example video):** [https://www.youtube.com/watch?v=dQI0OpzfVHw](https://www.youtube.com/watch?v=dQI0OpzfVHw)

> If you plan to stream your Kit app, look into **Omniverse Kit App Streaming**, **NVIDIA Cloud Functions (NVCF)**, and **Graphics Delivery Network (GDN)**.

---

## License & Contributions

* This repo’s source is provided **as‑is**; external contributions are not currently accepted.
* Omniverse software and materials are governed by the **NVIDIA Software License Agreement** and **product‑specific terms**.
* Kit SDK may collect anonymous usage data for diagnostics/performance; no personal information is collected per NVIDIA’s documentation. You can adjust telemetry settings in app configs.

---

### Acknowledgments

Thanks to the Omniverse team for the Kit SDK and templates, and to the open‑source community for 2D→3D workflows that accelerate prototyping.
