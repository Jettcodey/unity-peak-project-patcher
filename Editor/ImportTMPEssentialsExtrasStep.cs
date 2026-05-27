using System.IO;
using Cysharp.Threading.Tasks;
using Nomnom.UnityProjectPatcher.Editor.Steps;
using UnityEditor;
using UnityEngine;

namespace PeakModding.PeakProjectPatcher.Editor
{
    public struct ImportTMPEssentialsExtrasStep : IPatcherStep
    {
        public UniTask<StepResult> Run()
        {
            string tmpPath = Path.Combine(Application.dataPath, "TextMesh Pro");
            
            if (!Directory.Exists(tmpPath))
            {
                Debug.Log("Importing TMP Essentials & Extras...");
                
                // Unity 6000+ moved TMP resources into the uGUI package
                string tmpPackagePath = Path.GetFullPath("Packages/com.unity.ugui");
                string essentialsPath = Path.Combine(tmpPackagePath, "Package Resources", "TMP Essential Resources.unitypackage");
                string extrasPath = Path.Combine(tmpPackagePath, "Package Resources", "TMP Examples & Extras.unitypackage");

                if (File.Exists(essentialsPath))
                {
                    AssetDatabase.ImportPackage(essentialsPath, false);
                    AssetDatabase.ImportPackage(extrasPath, false);
                }
                else
                {
                    Debug.LogError("Failed to find TMP packages. Verify uGUI package is installed.");
                }
            }
            
            return UniTask.FromResult(StepResult.Success);
        }
        
        public void OnComplete(bool failed) { }
    }
}