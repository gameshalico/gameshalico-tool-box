using System.IO;
using UnityEditor;
using UnityEngine;

namespace ShalicoToolBox.Editor
{
    public class ProjectInitializer : EditorWindow
    {
        private static string s_projectName = "ProjectName";

        private readonly string[] _folders =
        {
            "Art/Materials", "Art/Models", "Art/Textures", "Art/Animations", "Art/Animators", "Art/Fonts",
            "Audio/Music", "Audio/SFX", "Code/Scripts", "Code/Shaders", "Code/Particles", "Editor/Scripts",
            "Levels/Scenes", "Levels/Prefabs", "Levels/UI", "Levels/ScriptableObjects", "Settings", "Documents"
        };

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Project Initializer");
            s_projectName = EditorGUILayout.TextField("Project Name", s_projectName);

            GUILayout.Space(10);
            if (GUILayout.Button("Initialize"))
            {
                CreateFolders();
            }
        }

        private void CreateFolders()
        {
            _folders.ForEach(folder =>
            {
                Directory.CreateDirectory($"{Application.dataPath}/{s_projectName}/{folder}");
                File.Create($"{Application.dataPath}/{s_projectName}/{folder}/.gitkeep");
            });
            AssetDatabase.Refresh();
        }

        [MenuItem("Tools/Shalico/Project Initializer")]
        private static void Open()
        {
            GetWindow<ProjectInitializer>("Project Initializer");
        }
    }
}