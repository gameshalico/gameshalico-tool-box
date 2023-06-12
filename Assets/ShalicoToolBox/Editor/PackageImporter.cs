using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;
using UnityEngine;
using PackageInfo = UnityEditor.PackageManager.PackageInfo;

namespace Shalico.ToolBox.Editor
{
    public class PackageImporter : EditorWindow
    {
        private static readonly string s_packagePrefsKey = "PackageImporter.PackageData";

        private static List<PackageData> s_packages;
        private string _newPackageIdentifier = "";
        private string _newPackageName = "";

        private void OnGUI()
        {
            foreach (PackageData package in s_packages)
            {
                EditorGUILayout.BeginHorizontal();
                package.shouldImport = EditorGUILayout.ToggleLeft(package.name, package.shouldImport);
                if (GUILayout.Button("Delete"))
                {
                    s_packages.Remove(package);
                    SavePackageData();
                }

                EditorGUILayout.EndHorizontal();
            }

            GUILayout.Space(20);
            GUILayout.Label("Add New Package", EditorStyles.boldLabel);
            _newPackageName = EditorGUILayout.TextField("Name", _newPackageName);
            _newPackageIdentifier = EditorGUILayout.TextField("Identifier", _newPackageIdentifier);
            if (GUILayout.Button("Add Package"))
            {
                s_packages.Add(new PackageData(_newPackageName, _newPackageIdentifier, false));
                _newPackageName = "";
                _newPackageIdentifier = "";
                SavePackageData();
            }

            GUILayout.Space(20);
            if (GUILayout.Button("Import Selected Packages"))
            {
                ListRequest listRequest = Client.List();

                while (!listRequest.IsCompleted)
                {
                }

                List<string> installedPackages = new();
                foreach (PackageInfo package in listRequest.Result)
                {
                    installedPackages.Add(package.name);
                }

                foreach (PackageData package in s_packages)
                {
                    if (package.shouldImport && !installedPackages.Contains(package.identifier))
                    {
                        ImportPackage(package.identifier);
                    }
                }
            }
        }

        [MenuItem("Window/Package Importer")]
        public static void ShowWindow()
        {
            GetWindow<PackageImporter>("Package Importer");
            LoadPackageData();
        }

        private static void LoadPackageData()
        {
            string jsonData = EditorPrefs.GetString(s_packagePrefsKey, "");

            if (string.IsNullOrEmpty(jsonData))
            {
                s_packages = new List<PackageData>();
            }
            else
            {
                s_packages = JsonUtility.FromJson<ListWrapper<PackageData>>(jsonData).list;
            }
        }

        private static void SavePackageData()
        {
            ListWrapper<PackageData> wrapper = new() { list = s_packages };

            string jsonData = JsonUtility.ToJson(wrapper);
            EditorPrefs.SetString(s_packagePrefsKey, jsonData);
        }

        private void ImportPackage(string packageName)
        {
            AddRequest request = Client.Add(packageName);

            while (!request.IsCompleted)
            {
            }

            if (request.Status == StatusCode.Success)
            {
                Debug.Log("Installed: " + request.Result.packageId);
            }
            else if (request.Status >= StatusCode.Failure)
            {
                Debug.Log("Failed to install package: " + packageName);
            }
        }
    }

    [Serializable]
    public class PackageData
    {
        public string name;
        public string identifier;
        public bool shouldImport;

        public PackageData(string name, string identifier, bool shouldImport)
        {
            this.name = name;
            this.identifier = identifier;
            this.shouldImport = shouldImport;
        }
    }

    [Serializable]
    public class ListWrapper<T>
    {
        public List<T> list;
    }
}