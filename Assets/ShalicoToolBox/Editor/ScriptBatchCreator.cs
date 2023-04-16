using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Shalico.ToolBox.Editor
{
    public static class ScriptBatchCreator
    {
        private const string DirectoryPath = "Assets/Plugins/ShalicoToolBox/Scripts/Generated";
        private const string ClassNamespace = "Shalico.ToolBox";

        public static ScriptGenerator CreateLayerClassGenerator()
        {
            string[] layerNames = UnityEditorInternal.InternalEditorUtility.layers;
            ScriptGenerator scriptGenerator = new() { ClassName = "LayerName", Namespace = ClassNamespace };
            layerNames.ForEach(
                layerName =>
                scriptGenerator.AddConstantField("int", layerName, LayerMask.NameToLayer(layerName).ToString())
                );
            layerNames.ForEach(
                layerName =>
                scriptGenerator.AddConstantField("int", layerName + "Mask", (1 << LayerMask.NameToLayer(layerName)).ToString())
                );
            return scriptGenerator;
        }
        public static ScriptGenerator CreateTagClassGenerator()
        {
            string[] tagNames = UnityEditorInternal.InternalEditorUtility.tags;
            ScriptGenerator scriptGenerator = new() { ClassName = "TagName", Namespace = ClassNamespace };
            tagNames.ForEach(
                tagName =>
                scriptGenerator.AddConstantField("string", tagName, $"\"{tagName}\"")
                );
            return scriptGenerator;
        }
        public static ScriptGenerator CreateSceneClassGenerator()
        {
            string[] scenePaths = EditorBuildSettings.scenes.Select(scene => scene.path).ToArray();
            ScriptGenerator scriptGenerator = new() { ClassName = "SceneName", Namespace = ClassNamespace };
            scenePaths.Select(path => Path.GetFileNameWithoutExtension(path)).ForEach(
                name =>
                scriptGenerator.AddConstantField("string", name, $"\"{name}\"")
                );
            scenePaths.ForEach(
                path =>
                scriptGenerator.AddConstantField("int", Path.GetFileNameWithoutExtension(path) + "Index", SceneManager.GetSceneByPath(path).buildIndex.ToString())
                );
            return scriptGenerator;
        }
        public static ScriptGenerator CreateSortingLayerClassGenerator()
        {
            string[] sortingLayerNames = SortingLayer.layers.Select(layer => layer.name).ToArray();
            ScriptGenerator scriptGenerator = new() { ClassName = "SortingLayerName", Namespace = ClassNamespace };
            sortingLayerNames.ForEach(
                sortingLayerName =>
                scriptGenerator.AddConstantField("int", sortingLayerName, SortingLayer.NameToID(sortingLayerName).ToString())
                );
            return scriptGenerator;
        }


        [MenuItem("Tools/Shalico/Create/Setting Class")]
        public static void GenerateAll()
        {
            CreateLayerClassGenerator().Generate(DirectoryPath);
            CreateTagClassGenerator().Generate(DirectoryPath);
            CreateSceneClassGenerator().Generate(DirectoryPath);
            CreateSortingLayerClassGenerator().Generate(DirectoryPath);

            AssetDatabase.Refresh();
        }
    }
}