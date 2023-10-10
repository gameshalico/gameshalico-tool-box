using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ShalicoReferenceGenerator.Editor
{
    public static class ScriptBatchCreator
    {
        private const string DirectoryPath = "Assets/Plugins/ShalicoReferenceGenerator/";
        private const string ClassNamespace = "ShalicoReferenceGenerator";

        private static ScriptGenerator CreateLayerClassGenerator()
        {
            var layerNames = InternalEditorUtility.layers;
            ScriptGenerator scriptGenerator = new() { ClassName = "LayerName", Namespace = ClassNamespace };

            foreach (var layerName in layerNames)
                scriptGenerator.AddConstantField("int", layerName, LayerMask.NameToLayer(layerName).ToString());
            foreach (var layerName in layerNames)
                scriptGenerator.AddConstantField("int", layerName + "Mask",
                    (1 << LayerMask.NameToLayer(layerName)).ToString());

            return scriptGenerator;
        }

        private static ScriptGenerator CreateTagClassGenerator()
        {
            var tagNames = InternalEditorUtility.tags;
            ScriptGenerator scriptGenerator = new() { ClassName = "TagName", Namespace = ClassNamespace };

            foreach (var tagName in tagNames)
                scriptGenerator.AddConstantField("string", tagName, $"\"{tagName}\"");

            return scriptGenerator;
        }

        private static ScriptGenerator CreateSceneClassGenerator()
        {
            var scenePaths = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();

            ScriptGenerator scriptGenerator = new() { ClassName = "SceneName", Namespace = ClassNamespace };

            foreach (var path in scenePaths)
            {
                var sceneName = Path.GetFileNameWithoutExtension(path);
                scriptGenerator.AddConstantField("string", sceneName, $"\"{sceneName}\"");
            }

            foreach (var path in scenePaths)
            {
                var sceneName = Path.GetFileNameWithoutExtension(path);
                scriptGenerator.AddConstantField("int", sceneName + "Index",
                    SceneManager.GetSceneByPath(path).buildIndex.ToString());
            }

            return scriptGenerator;
        }

        private static ScriptGenerator CreateSortingLayerClassGenerator()
        {
            var sortingLayerNames = SortingLayer.layers.Select(layer => layer.name).ToArray();
            ScriptGenerator scriptGenerator = new() { ClassName = "SortingLayerName", Namespace = ClassNamespace };

            foreach (var sortingLayerName in sortingLayerNames)
                scriptGenerator.AddConstantField("int", sortingLayerName,
                    SortingLayer.NameToID(sortingLayerName).ToString());

            return scriptGenerator;
        }


        [MenuItem("Tools/Shalico/Reference Generator/Create/All")]
        internal static void GenerateAll()
        {
            CreateLayerClassGenerator().Generate(DirectoryPath);
            CreateTagClassGenerator().Generate(DirectoryPath);
            CreateSceneClassGenerator().Generate(DirectoryPath);
            CreateSortingLayerClassGenerator().Generate(DirectoryPath);

            AssetDatabase.Refresh();
        }
    }
}