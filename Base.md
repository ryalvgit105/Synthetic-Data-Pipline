1. Unity Perception (Automaatic labeler and randomizer)
https://youtu.be/mkVE2Yhe454?si=Neb-Hwny9TC4ZjZQ

Preperation:

(Render error)

Do this exactly

Open Edit → Project Settings → Quality.

You’ll see several rows (Low, Medium, Balanced, High Fidelity, etc.).

For each row, check the Render Pipeline Asset field on the right.

Click the circle and open that asset in the Inspector.

Under Rendering → Lit Shader Mode, set to Both or Forward Only.

Save.

Repeat for every quality level listed in the warnings.


(async shader error)

Open Edit → Project Settings → Player.

Under Other Settings, find Asynchronous Shader Compilation.

Uncheck it.

This forces all shaders to compile synchronously when needed (during load), so no background GPU hitching occurs.


2.Military Base Pack (Test Enviroment)
https://assetstore.unity.com/search#q=military%20base

(Fix purple materials when importing a Built-In Render Pipeline asset into an HDRP project)

step 1 Confirm HDRP Is Active


In Unity, go to
Edit → Project Settings → Graphics.


Ensure the Scriptable Render Pipeline Settings field points to your HDRP Asset (e.g. High Definition Render Pipeline Asset).


If not, create one:
Assets → Create → Rendering → High Definition Render Pipeline Asset
then assign it in that field.


Step 2: Convert Materials Automatically
Unity provides a built-in upgrader.


Open menu:
Edit → Render Pipeline → HDRP → Materials → Convert All Built-in Materials to HDRP


Wait until Unity finishes conversion.
This converts Standard Shader materials to HDRP Lit materials.


If only specific folders need conversion:
Edit → Render Pipeline → HDRP → Materials → Convert Selected Materials to HDRP



Step 3: Upgrade Lighting and Skybox


Go to Edit → Render Pipeline → HDRP → Upgrade Project Materials to HDRP (optional global fix).


Replace any old Skybox materials:


Open Window → Rendering → Lighting → Environment.


Under Skybox Material, select an HDRI Sky or Procedural Sky from HDRP examples.



Step 4. Check Prefabs and Models


Open imported prefabs that still show purple.


In the Inspector, look at the Mesh Renderer → Materials field.


If materials still reference “Standard,” click the gear → Select Material in Project, then right-click it → Convert to HDRP Material.



Step 5. Fix Textures Manually (if needed)
If conversion fails for some materials:


Open each purple material.


Change Shader dropdown to HDRP/Lit.


Reassign textures:


Base Map → Albedo


Mask Map (combine metallic/smoothness/occlusion if available)


Normal Map → Normal


Emission Map → Emission

Step 6. Re-generate or Re-import

If some assets still render incorrectly, right-click the affected folder → Reimport.

Run conversion again if new materials appear.

Step 7. (Optional) Automate Future Imports
For recurring pipeline mismatches:


Go to Edit → Project Settings → Graphics → Default Material.


Set default to HDRP/Lit so new models use HDRP automatically.

Result
All imported assets from the military base pack now use HDRP shaders and materials. The purple “missing shader” issue disappears, and lighting behaves correctly under HDRP.


3.

4. Unity Coplay (LLM integration that allows for scene and object creation)
https://github.com/CoplayDev/coplay-unity-plugin


