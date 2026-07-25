using System.IO;
using Cysharp.Threading.Tasks;
using Nomnom.UnityProjectPatcher.Editor.Steps;
using UnityEditor;
using UnityEngine;

namespace PeakModding.PeakProjectPatcher.Editor {
    public struct DisableHBAOFeatureStep : IPatcherStep {
        public UniTask<StepResult> Run() {
            DisableFeature();
            return UniTask.FromResult(StepResult.Success);
        }

        static void DisableFeature() {
            var folder = GetTargetMonoBehaviourFolder();
            if (string.IsNullOrEmpty(folder)) return;

            var assetPath = Path.Combine(folder, "HBAO_1.asset");
            if (!File.Exists(assetPath)) return;

            const string enabledLine = "m_Active: 1";
            var content = File.ReadAllText(assetPath);
            if (!content.Contains(enabledLine)) return;

            File.WriteAllText(assetPath, content.Replace(enabledLine, "m_Active: 0"));
            Debug.Log("Disabled the HBAO renderer feature");
        }

        static string GetTargetMonoBehaviourFolder() {
            string projectRoot = Directory.GetParent(Application.dataPath).FullName;
            string assetRipperPath = Path.Combine(projectRoot, "AssetRipperOutput", "ExportedProject", "Assets", "MonoBehaviour");
            if (Directory.Exists(assetRipperPath)) return assetRipperPath;

            string assetsPath = Path.Combine(Application.dataPath, "PEAK", "Game", "ScriptableObjects");
            if (Directory.Exists(assetsPath)) return assetsPath;

            return null;
        }

        public void OnComplete(bool failed) { }
    }
}
