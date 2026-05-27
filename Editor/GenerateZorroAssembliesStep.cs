using System;
using System.Collections.Generic;
using System.IO;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nomnom.UnityProjectPatcher.Editor.Steps;
using UnityEditor;
using UnityEngine;

namespace PeakModding.PeakProjectPatcher.Editor {
    public struct GenerateZorroAssembliesStep : IPatcherStep {
        
        [MenuItem("Tools/PEAK Project Patcher/Generate Zorro Assembly Definitions")]
        static void MenuItem() {
            GenerateDefinitions();
            EditorUtility.RequestScriptReload();
        }

        static readonly Dictionary<string, string[]> Dependencies = new() {
            { "Zorro.AutoLOD", Array.Empty<string>() },
            { "Zorro.ControllerSupport", new [] { 
                "Unity.InputSystem", 
                "Unity.TextMeshPro", 
                "Zorro.Core.Runtime", 
                "Zorro.UI.Runtime" 
            } },
            { "Zorro.Core.Runtime", new [] { 
                "Unity.Mathematics", 
                "Unity.Burst", 
                "Unity.Collections", 
                "Unity.InputSystem" 
            } },
            { "Zorro.JiggleBones", new [] { 
                "Unity.Mathematics", 
                "Unity.Burst", 
                "Unity.Collections", 
                "Zorro.Core.Runtime" 
            } },
            { "Zorro.Settings.Runtime", new [] { 
                "Unity.TextMeshPro", 
                "Unity.Localization", 
                "Unity.Mathematics", 
                "Zorro.Core.Runtime" 
            } },
            { "Zorro.UI.Runtime", new [] { 
                "Unity.TextMeshPro", 
                "Zorro.Core.Runtime" 
            } },
            { "Zorro.PhotonUtility", new [] { 
                "Photon3Unity3D", 
                "PhotonRealtime", 
                "PhotonUnityNetworking", 
                "Zorro.Core.Runtime" 
            } },
        };
        
        public UniTask<StepResult> Run() {
            GenerateDefinitions();
            
            return UniTask.FromResult(StepResult.Success);
        }

        static void GenerateDefinitions() {
            var scriptsFolder = GetTargetScriptsFolder();
            if (string.IsNullOrEmpty(scriptsFolder)) return;
            
            foreach (var (assembly, dependencies) in Dependencies) {
                var folderPath = Path.Combine(scriptsFolder, assembly);
                if (!Directory.Exists(folderPath)) continue;

                var asmdefPath = Path.Combine(folderPath, $"{assembly}.asmdef");
                var asmDef = new Dictionary<string, object> {
                    { "name", assembly },
                    { "references", dependencies },
                    { "allowUnsafeCode", true }
                };
                
                File.WriteAllText(asmdefPath, JsonConvert.SerializeObject(asmDef, Formatting.Indented));
            }

            CleanupGeneratedJobFiles(scriptsFolder);

            string assemblyFolder = Path.Combine(scriptsFolder, "Assembly-CSharp");
            if (Directory.Exists(assemblyFolder)) {
                FixMapBaker(assemblyFolder);
                FixKnotMaker(assemblyFolder);
            }
        }

        static string GetTargetScriptsFolder() {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string assetRipperPath = Path.Combine(projectRoot, "AssetRipperOutput", "ExportedProject", "Assets", "Scripts");
            if (Directory.Exists(assetRipperPath)) return assetRipperPath;
            
            string assetsPath = Path.Combine(Application.dataPath, "PEAK", "Game", "Scripts");
            if (Directory.Exists(assetsPath)) return assetsPath;

            return null;
        }

        static void CleanupGeneratedJobFiles(string scriptsFolder) {
            string[] files = Directory.GetFiles(scriptsFolder, "__JobReflectionRegistrationOutput__*.cs", SearchOption.AllDirectories);
            foreach (var file in files) File.Delete(file);
        }

        static void FixMapBaker(string assemblyFolder) {
            var mapBakerPath = Path.Combine(assemblyFolder, "MapBaker.cs");
            if (!File.Exists(mapBakerPath)) return;

            var content = File.ReadAllText(mapBakerPath);

            // If System.IO is alreay existing the file has already been patched.
            if (content.Contains("using System.IO;")) return;

            // Switch to System.IO.Path as we're missing? the Zorro.Core.Editor stuff 
            content = content.Replace("using Zorro.Core.Editor;", "using System.IO;");

            string oldStuff = "string result = PathUtil.WithoutExtensions(PathUtil.GetFileName(ScenePaths[levelIndex]));";
            string newStuff = "string result = Path.GetFileNameWithoutExtension(ScenePaths[levelIndex]);";

            if (content.Contains(oldStuff)) {
                content = content.Replace(oldStuff, newStuff);
            }

            File.WriteAllText(mapBakerPath, content);
            Debug.Log("Successfully patched MapBaker.cs to use System.IO.");
        }

        static void FixKnotMaker(string assemblyFolder) {
            var path = Path.Combine(assemblyFolder, "Knot", "KnotMaker.cs");
            if (File.Exists(path)) {
                var content = File.ReadAllText(path);
                content = content.Replace("quality:", "");
                File.WriteAllText(path, content);
            }
        }

        public void OnComplete(bool failed) { }
    }
}