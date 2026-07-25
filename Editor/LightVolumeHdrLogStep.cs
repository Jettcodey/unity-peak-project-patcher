using System.IO;
using Cysharp.Threading.Tasks;
using Nomnom.UnityProjectPatcher.Editor.Steps;
using UnityEditor;
using UnityEngine;

namespace PeakModding.PeakProjectPatcher.Editor {
    public struct LightVolumeHdrLogStep : IPatcherStep {
        public UniTask<StepResult> Run() {
            RemoveLogs();
            return UniTask.FromResult(StepResult.Success);
        }

        static void RemoveLogs() {
            var scriptsFolder = GetTargetScriptsFolder();
            if (string.IsNullOrEmpty(scriptsFolder)) return;

            var filePath = Path.Combine(scriptsFolder, "LightVolume.Runtime", "Peak", "CompressableLightMap.cs");
            if (!File.Exists(filePath)) return;

            const string logLine = "Debug.LogError(\"TODO: HANDLE HDR COMPRESSION AT RUNTIME\");";
            var content = File.ReadAllText(filePath);
            if (!content.Contains(logLine)) return;

            File.WriteAllText(filePath, content.Replace(logLine, ""));
            Debug.Log("Stripped the HDR TODO log spam");
        }

        static string GetTargetScriptsFolder() {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string assetRipperPath = Path.Combine(projectRoot, "AssetRipperOutput", "ExportedProject", "Assets", "Scripts");
            if (Directory.Exists(assetRipperPath)) return assetRipperPath;

            string assetsPath = Path.Combine(Application.dataPath, "PEAK", "Game", "Scripts");
            if (Directory.Exists(assetsPath)) return assetsPath;

            return null;
        }

        public void OnComplete(bool failed) { }
    }
}
