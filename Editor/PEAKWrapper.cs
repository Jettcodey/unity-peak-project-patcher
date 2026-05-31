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
            stepPipeline.InsertLast(new ImportTextMeshProStep());  // Works again
            stepPipeline.InsertLast(new GenerateGitIgnoreStep());
            stepPipeline.InsertLast(new GenerateReadmeStep());
            stepPipeline.InsertLast(new PackagesInstallerStep());  // restart and recompile
            stepPipeline.InsertLast(new CacheProjectCatalogueStep());
            stepPipeline.InsertLast(new AssetRipperStep());
            // Need to investigate if these 2 commented steps can fix some of the remaining issues we have
            // stepPipeline.InsertLast(new PromptUserWithAddressablePluginStep());
            // stepPipeline.InsertLast(new AddressablesGuidRemapperStep());
            stepPipeline.InsertLast(new CopyGamePluginsStep());  // recompile
            stepPipeline.InsertLast(new GeneratePhotonAssembliesStep());
            stepPipeline.InsertLast(new GenerateZorroAssembliesStep());
            // The currently missing references are very likely caused by not copying all needed decomped Assemblies
            stepPipeline.InsertLast(new CopyExplicitScriptFolderStep());  //restart 
            stepPipeline.InsertLast(new EnableUnsafeCodeStep());  //recompile
            stepPipeline.InsertLast(new CopyProjectSettingsStep(allowUnsafeCode: true));  //restart
            stepPipeline.InsertLast(new GuidRemapperStep());
            stepPipeline.InsertLast(new CopyAssetRipperExportToProjectStep());  //restart (throws safe mode error most of the time here)
            stepPipeline.InsertLast(new FixProjectFileIdsStep());
            stepPipeline.InsertLast(new InjectURPAssetsStep());  // was missing since the injection steps needed to be updated
            stepPipeline.InsertLast(new SortAssetTypesSteps());
            stepPipeline.InsertLast(new RestartEditorStep());  //restart (who would've thought lol)
			
			stepPipeline.SetInputSystem(InputSystemType.Both);
            stepPipeline.SetGameViewResolution("16:9");
            stepPipeline.OpenSceneAtEnd("Title");  // For some reason opens the Pretitle.unity file?
        }
    }
}