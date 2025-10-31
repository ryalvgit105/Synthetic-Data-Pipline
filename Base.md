# Unity Synthetic Data Pipeline Setup  
Automated Labeling, Randomization, and LLM-Driven Scene Control

This repository documents a complete workflow for creating a synthetic data pipeline in Unity using the Perception Package, HDRP, and Coplay (LLM Integration).  
It enables automated object labeling, scene randomization, and AI-driven environment generation.

---

## 1. Unity Perception Setup  
Automatic Labeler and Randomizer  
Tutorial: [Unity Perception Setup](https://youtu.be/mkVE2Yhe454?si=Neb-Hwny9TC4ZjZQ)

### Preparation Steps

#### A. Fix Render Errors on UNITY
1. Open **Edit → Project Settings → Quality**  
2. For each Quality Level (Low, Medium, High, etc.):  
   - Locate **Render Pipeline Asset** → open it in **Inspector**  
   - Under **Rendering → Lit Shader Mode**, set to **Both** or **Forward Only**  
   - Save changes  
3. Repeat for all quality levels listed in the warnings.

#### B. Fix Asynchronous Shader Compilation Errors ON UNITY 
1. Open **Edit → Project Settings → Player**  
2. Under **Other Settings**, find **Asynchronous Shader Compilation**  
3. **Uncheck** this option  
   - This forces all shaders to compile synchronously during load, preventing GPU hitching.

---

## 2. Military Base Pack (Test Environment)  
Asset Store: [Military Base Pack](https://assetstore.unity.com/search#q=military%20base)

### Fix “Purple Materials” on UNITY (Built-In → HDRP Conversion)

#### Step 1. Confirm HDRP Is Active
- Go to **Edit → Project Settings → Graphics**  
- Ensure **Scriptable Render Pipeline Settings** → assigned to your **HDRP Asset**  
- If missing:  
  - Create one: **Assets → Create → Rendering → High Definition Render Pipeline Asset**  
  - Assign it under **Graphics Settings**

#### Step 2. Convert Materials Automatically
- Use Unity’s upgrader:  
  **Edit → Render Pipeline → HDRP → Materials → Convert All Built-in Materials to HDRP**  
- For specific folders:  
  **Edit → Render Pipeline → HDRP → Materials → Convert Selected Materials to HDRP**

#### Step 3. Upgrade Lighting and Skybox
- Run global upgrade:  
  **Edit → Render Pipeline → HDRP → Upgrade Project Materials to HDRP**  
- Open **Window → Rendering → Lighting → Environment**  
- Replace **Skybox Material** with **HDRI Sky** or **Procedural Sky** from HDRP examples.

#### Step 4. Check Prefabs and Models
- Open prefabs with purple meshes.  
- In **Inspector → Mesh Renderer → Materials**, select any **Standard** materials.  
- Click the gear → **Select Material in Project** → right-click → **Convert to HDRP Material**.

#### Step 5. Fix Textures Manually (if needed)
1. Open affected material.  
2. Set **Shader → HDRP/Lit**.  
3. Reassign textures:  
   - **Base Map → Albedo**  
   - **Mask Map → Metallic/Smoothness/Occlusion (combined)**  
   - **Normal Map → Normal**  
   - **Emission Map → Emission**

#### Step 6. Re-import Assets
- Right-click the folder → **Reimport**  
- Re-run material conversion if new materials appear.

#### Step 7. Automate Future Imports (Optional)
- Open **Edit → Project Settings → Graphics**  
- Under **Default Material**, set to **HDRP/Lit**  
- Ensures future assets import with correct shaders.

Result: All imported materials render correctly under HDRP. No more purple materials.

---

## 3. Unity Coplay Integration (LLM-Driven Scene Creation)
Repository: [Coplay-Unity-Plugin](https://github.com/CoplayDev/coplay-unity-plugin)

### Overview
Coplay connects Unity to a language model for automated:
- Scene and object creation via text or voice commands  
- Script generation for object manipulation  
- Dynamic environment adjustments during runtime

### Basic Setup
1. Clone the repository:  
   ```bash
   git clone https://github.com/CoplayDev/coplay-unity-plugin.git
