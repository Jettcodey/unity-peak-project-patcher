using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using Cysharp.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Nomnom.UnityProjectPatcher.Editor.Steps;
using UnityEditor;
using UnityEngine;

namespace PeakModding.PeakProjectPatcher.Editor {
    public struct GeneratePhotonAssembliesStep : IPatcherStep {
        
        [MenuItem("Tools/PEAK Project Patcher/Generate Photon Assembly Definitions")]
        static void MenuItem() {
            GenerateDefinitions();
            EditorUtility.RequestScriptReload();
        }

        static readonly Dictionary<string, string[]> Dependencies = new() {
            { "Photon3Unity3D", Array.Empty<string>() },
            { "PhotonChat", new [] { 
                "Photon3Unity3D" 
            } },
            { "PhotonRealtime", new [] { 
                "Photon3Unity3D" 
            } },
            { "PhotonUnityNetworking", new [] { 
                "Photon3Unity3D", 
                "PhotonRealtime" 
            } },
            { "PhotonUnityNetworking.Utilities", new [] { 
                "Photon3Unity3D", 
                "PhotonRealtime", 
                "PhotonUnityNetworking" 
            } },
            { "PhotonVoice.API", new [] { 
                "Photon3Unity3D", 
                "PhotonRealtime", 
                "Zorro.Core.Runtime" 
            } },
            { "PhotonVoice.PUN", new [] { 
                "Photon3Unity3D", 
                "PhotonRealtime", 
                "PhotonUnityNetworking", 
                "PhotonVoice", 
                "PhotonVoice.API", 
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

            string assemblyFolder = Path.Combine(scriptsFolder, "Assembly-CSharp");
            if (Directory.Exists(assemblyFolder)) {
                FixAssetRipperNamespaceCollisions(assemblyFolder);
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

        static void FixAssetRipperNamespaceCollisions(string assemblyFolder) {
            string[] files = Directory.GetFiles(assemblyFolder, "*.cs", SearchOption.AllDirectories);
            
            var pattern1 = new Regex(@"\b(OnPlayerEnteredRoom|OnPlayerLeftRoom|OnMasterClientSwitched|OnPlayerPropertiesUpdate)(\s*\(\s*)Player\b");
            var pattern2 = new Regex(@"\b(OnOwnershipRequest|OnOwnershipTransfered|OnOwnershipTransferFailed)(\s*\([^,]+,\s*)Player\b");

            foreach (var file in files) {
                string content = File.ReadAllText(file);
                bool modified = false;

                if (pattern1.IsMatch(content)) {
                    content = pattern1.Replace(content, "$1$2Photon.Realtime.Player");
                    modified = true;
                }

                if (pattern2.IsMatch(content)) {
                    content = pattern2.Replace(content, "$1$2Photon.Realtime.Player");
                    modified = true;
                }

                if (modified) File.WriteAllText(file, content);
            }
        }

        public void OnComplete(bool failed) { }
    }
}