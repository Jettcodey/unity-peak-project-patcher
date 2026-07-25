using System.IO;
using Cysharp.Threading.Tasks;
using Nomnom.UnityProjectPatcher.Editor.Steps;
using UnityEditor;
using UnityEngine;

namespace PeakModding.PeakProjectPatcher.Editor {
    public struct DOTweenVisibilityStep : IPatcherStep {
        public UniTask<StepResult> Run() {
            MakeVisible();
            return UniTask.FromResult(StepResult.Success);
        }

        static void MakeVisible() {
            var scriptsFolder = GetTargetScriptsFolder();
            if (string.IsNullOrEmpty(scriptsFolder)) return;

            var dotweenFolder = Path.Combine(scriptsFolder, "DOTween");
            if (!Directory.Exists(dotweenFolder)) return;

            var asmInfoPath = Path.Combine(dotweenFolder, "AssemblyInfo.cs");
            if (File.Exists(asmInfoPath) && File.ReadAllText(asmInfoPath).Contains("InternalsVisibleTo")) return;

            File.WriteAllText(asmInfoPath, "using System.Runtime.CompilerServices;\n\n[assembly: InternalsVisibleTo(\"DOTweenPro\")]\n");
            Debug.Log("DOTweenPro can see DOTween internals again");
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
