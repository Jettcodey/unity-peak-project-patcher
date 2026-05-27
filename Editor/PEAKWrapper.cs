using Nomnom.UnityProjectPatcher.Editor;
using Nomnom.UnityProjectPatcher.Editor.Steps;

namespace PeakModding.PeakProjectPatcher.Editor 
{
    [UPPatcher("com.peakmodding.unity-peak-project-patcher")]
    public static class PEAKWrapper 
	{
        public static void GetSteps(StepPipeline stepPipeline) 
		{
			stepPipeline.Steps.Clear();

            stepPipeline.InsertLast(new GenerateDefaultProjectStructureStep());
            // stepPipeline.InsertLast(new ImportTextMeshProStep());  // Doesnt work with Unity 6000+
            stepPipeline.InsertLast(new ImportTMPEssentialsExtrasStep());  // Important wrapper script to replace the above broken TMP import step for unity 6000+
            stepPipeline.InsertLast(new GenerateGitIgnoreStep());
            // stepPipeline.InsertLast(new GenerateReadmeStep());  // not needed imo so commented out
            stepPipeline.InsertLast(new PackagesInstallerStep());  // restart and recompile
            stepPipeline.InsertLast(new CacheProjectCatalogueStep());
            stepPipeline.InsertLast(new AssetRipperStep());
            stepPipeline.InsertLast(new CopyGamePluginsStep());  // recompile
            stepPipeline.InsertLast(new GeneratePhotonAssembliesStep());
            stepPipeline.InsertLast(new GenerateZorroAssembliesStep());
            stepPipeline.InsertLast(new CopyExplicitScriptFolderStep());  //restart
            stepPipeline.InsertLast(new EnableUnsafeCodeStep());  //recompile
            stepPipeline.InsertLast(new CopyProjectSettingsStep(allowUnsafeCode: true));  //restart
            stepPipeline.InsertLast(new GuidRemapperStep());
            stepPipeline.InsertLast(new CopyAssetRipperExportToProjectStep());  //restart (throws safe mode error most of the time here)
            stepPipeline.InsertLast(new FixProjectFileIdsStep());
            stepPipeline.InsertLast(new SortAssetTypesSteps());
            stepPipeline.InsertLast(new RestartEditorStep());  //restart (who would've thought lol)
			
			stepPipeline.SetInputSystem(InputSystemType.Both);
            stepPipeline.SetGameViewResolution("16:9");
            stepPipeline.OpenSceneAtEnd("Title");  // For some reason opens the Pretitle.unity file?
        }
    }
}