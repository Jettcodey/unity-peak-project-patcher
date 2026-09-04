<div align="center">
  <h1>PEAK Project Patcher</h1>

<img src="images/banner.png" width="306.67" height="143.3"/>

  <p>
    A game wrapper that generates a Unity project from PEAK's build <strike>that can be playable in-editor</strike> <-- Sadly not yet.
  </p>
</div>

# Table of Contents

- [About the Project](#about-the-project)
- [Prerequisites](#prerequisites)
- [Unity Project Setup](#unity-project-setup)
- [Run Project Patcher](#run-project-patcher)
- [FAQ](#faq)

## About the Project
This tool is a game wrapper on top of the [Unity Project Patcher Fork](https://github.com/Jettcodey/unity-project-patcher) by Jettcodey.\
This wrapper allows you to rip a build of PEAK with all its extracted assets/scripts/etc to then generate a project for usage in the Unity editor.

> [!IMPORTANT]  
> This tool does not distribute game files. It simply works off of your copy of the game!
>
> Also, this tool is for **personal** use only. Do not re-distrubute game files to others.

Current Patcher version is `v0.2.5` for game version `2.04.b and up`\
The finished PEAK Unity Project functionality and custom creations are limited to some extent. Make sure to check the list below.
<details>
<summary><strong>List of Current Limitations (Click to Expand)</strong></summary>

- Editor Play-Mode Starts but UI Elements and Interaction is Limited to non working.
- GameObjects, Prefabs, and ScriptableObjects may have missing references.
- Project uses Dummy Shaders and needs Replacements.
  - Replacements need to be made or you can try importing shaders gathered following this guide on the PEAK Modding Discord Server:
    https://discord.com/channels/1363179626435707082/1444836081559408802

</details>

### Prerequisites
Download and Install the following:
- **[Git](https://git-scm.com/downloads)**
- **[Unity Hub](https://unity.com/download)**
- **[Unity Editor 6000.3.15f1](https://unity.com/releases/editor/whats-new/6000.3.15f1)**
- **[.NET SDK 10.0](https://dotnet.microsoft.com/en-us/download/dotnet/10.0)**

> [!IMPORTANT]
> Make a copy of your PEAK installation and open the `PEAK_Data` folder inside the copied directory.
> In this folder, locate the `level` and `sharedassets` files numbered **0 through 5**. Delete all other `level` and `sharedassets` files (numbered 6 and upwards).
> **Skipping this step will increase the patch time significantly.**
> 
> <details>
> <summary><strong>Expected <code>PEAK_Data</code> folder structure (Click to Expand)</strong></summary>
> 
> ```text
> PEAK_Data/
>   ├── Managed/
>   ├── Plugins/
>   ├── Resources/
>   ├── StreamingAssets/
>   ├── app.info
>   ├── boot.config
>   ├── globalgamemanagers
>   ├── globalgamemanagers.assets
>   ├── globalgamemanagers.assets.resS
>   ├── level0
>   ├── level1
>   ├── level2
>   ├── level3
>   ├── level4
>   ├── level5
>   ├── resources.assets
>   ├── resources.assets.resS
>   ├── resources.resource
>   ├── RuntimeInitializeOnLoads.json
>   ├── ScriptingAssemblies.json
>   ├── sharedassets0.assets
>   ├── sharedassets0.assets.resS
>   ├── sharedassets0.resource
>   ├── sharedassets1.assets
>   ├── sharedassets1.assets.resS
>   ├── sharedassets1.resource
>   ├── sharedassets2.assets
>   ├── sharedassets2.assets.resS
>   ├── sharedassets3.assets
>   ├── sharedassets3.assets.resS
>   ├── sharedassets3.resource
>   ├── sharedassets4.assets
>   ├── sharedassets4.assets.resS
>   ├── sharedassets4.resource
>   └── sharedassets5.assets
> ```
> </details>

### Unity Project Setup
Create a new Unity project with the following configuration:
  - **Editor Version: `6000.3.15f1`**
  - **Project Location**: Local Project
  - **Template**: Universal 3D (URP/SRP)

Once Unity fully opens, navigate to the menubar and click **`Window > Package Management > Package Manager`**.\
Click the **`🞢`** button in the top left of the Package Manager and choose **`Add package from git URL...`**
and install the required Unity Packages by pasting the following links **one by one** into the Text field:\
**Unity Project Patcher:**
  ```
  https://github.com/Jettcodey/unity-project-patcher.git
  ```
**Unity Project Patcher BepInEx (Optional):**
  ```
  https://github.com/Jettcodey/unity-project-patcher-bepinex.git
  ```
**Unity PEAK Project Patcher:**
  ```
  https://github.com/Jettcodey/unity-peak-project-patcher.git
  ```
Please make sure to **only use** these git links when setting up the PEAK Unity Project Patcher!
### Run Project Patcher
Navigate to the Menubar and click **`Tools > Unity Project Patcher > Configs > UPPatcherUserSettings`**.
- You will now see new options in the Inspector panel. Leave all the Pre filled fields as they are and only Enter the path to your **prepared/copied** game folder.

Back to the Menubar and open the Patcher Window by going to **`Tools > Unity Project Patcher > Open Window`**.
- (Optional) If the BepInEx package is installed:\
  Click on **`Enable BepInEx`** at the bottom of the window and wait for the process to finish.

Press **`Run Patcher`** to start the patching process.

**Expected patch behavior:**
- Fresh patch time: ~45-60 minutes.
- Unity will restart approximately 5-6 times during the process.
- Simply click **OK** on the 4 popups when prompted after starting the process.
- For "Script Updating Consent" select: "Yes, for these and other files that might be found later".
## FAQ
**How do I get rid of the "No cameras rendering" warning?**\
Right click the `Game` window and uncheck the checkbox labeled "Warn if no cameras rendering"\
For more questions, see core project's FAQ: https://github.com/Jettcodey/unity-project-patcher#faq
## Credits
Initial Unity Peak Project Patcher development by [Jettcodey](https://github.com/Jettcodey)\
The **`Unity PEAK Project Patcher`** would not have been possible without the prior work on the [R.E.P.O. Project Patcher](https://github.com/ZehsTeam/unity-repo-project-patcher) by [Kesomannen](https://github.com/Kesomannen/) and [ZehsTeam](https://github.com/ZehsTeam), which was used as a template, along with the inclusion of a slightly modified version of the [GeneratePhotonAssembliesStep.cs](https://github.com/ZehsTeam/unity-repo-project-patcher/blob/master/Editor/GeneratePhotonAssembliesStep.cs) file.\
Also a Huge thanks to all the Members of the [PEAK Modding Discord Server](https://discord.gg/SAw86z24rB). All the previously documented findings and guides for ripping the game assets were a very useful resource while developing this Unity Project Patcher wrapper.
