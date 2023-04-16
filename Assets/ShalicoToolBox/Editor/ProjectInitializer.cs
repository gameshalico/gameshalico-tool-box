using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Shalico.ToolBox.Editor
{
    public class ProjectInitializer : EditorWindow
    {
        private static string s_projectName = "ProjectName";

        private string[] _folders = new string[] {
            "Art/Materials",
            "Art/Models",
            "Art/Textures",
            "Art/Animations",
            "Art/Animators",
            "Art/Fonts",

            "Audio/Music",
            "Audio/SFX",

            "Code/Scripts/Core/Managers",
            "Code/Scripts/Core/Utilities",
            "Code/Scripts/GamePlay",
            "Code/Scripts/UI",

            "Code/Shaders",
            "Code/Particles",

            "Editor/Scripts",

            "Levels/Scenes",
            "Levels/Prefabs",
            "Levels/UI",
            "Levels/ScriptableObjects",

            "Settings",
            "Documents"
        };

        private void CreateFolders()
        {
            _folders.ForEach(folder =>
            {
                Directory.CreateDirectory($"{Application.dataPath}/{s_projectName}/{folder}");
            });
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Shalico/Project Initializer")]
        private static void Open()
        {
            GetWindow<ProjectInitializer>("Project Initializer");
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Project Initializer");
            s_projectName = EditorGUILayout.TextField("Project Name", s_projectName);
            GUILayout.Space(10);
            if (GUILayout.Button("Initialize"))
            {
                CreateFolders();
                ScriptBatchCreator.GenerateAll();
            }
        }
    }
}