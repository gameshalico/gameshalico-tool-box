using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace Shalico.ToolBox.Editor
{
    public class PackageImporter : EditorWindow
    {
        private static readonly List<PackageData> s_packages = new()
        {
            new PackageData
            {
                Identifier = "https://github.com/neuecc/UniRx.git?path=Assets/Plugins/UniRx/Scripts",
                ShouldImport = true
            },
            new PackageData
            {
                Identifier = "https://github.com/Cysharp/UniTask.git?path=src/UniTask/Assets/Plugins/UniTask",
                ShouldImport = true
            },
            new PackageData
            {
                Identifier = "https://github.com/hadashiA/VContainer.git?path=VContainer/Assets/VContainer",
                ShouldImport = true
            }
        };

        private void OnEnable()
        {
            Debug.Log("Package Importer Enabled");
        }

        private void OnGUI()
        {
            foreach (PackageData package in s_packages)
            {
                package.ShouldImport = EditorGUILayout.ToggleLeft(package.Identifier, package.ShouldImport);
            }

            if (GUILayout.Button("Import Selected Packages"))
            {
                foreach (PackageData package in s_packages)
                {
                    if (package.ShouldImport)
                    {
                        ImportPackage(package.Identifier);
                    }
                }
            }
        }


        [MenuItem("Window/Package Importer")]
        public static void ShowWindow()
        {
            GetWindow<PackageImporter>("Package Importer");
        }

        private void ImportPackage(string identifier)
        {
            AddRequest request = Client.Add(identifier);

            while (!request.IsCompleted)
            {
            }

            if (request.Status == StatusCode.Success)
            {
                Debug.Log("Installed: " + request.Result.packageId);
            }
            else if (request.Status >= StatusCode.Failure)
            {
                Debug.Log("Failed to install package: " + identifier);
            }
        }
    }

    public record PackageData
    {
        public string Identifier { get; init; }
        public bool ShouldImport { get; set; }
    }
}