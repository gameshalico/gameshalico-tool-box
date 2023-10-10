using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ShalicoHierarchyEnhancer.Editor
{
    internal static class HierarchyHighlight
    {
        private static readonly Color HighlightColor = new(0.5f, 0.5f, 0.0f, 0.5f);

        public static bool IsHighlighted(GameObject gameObject)
        {
            return gameObject.name.StartsWith("+++");
        }

        public static void Fill(TreeViewItem viewItem, GameObject gameObject, Rect selectionRect)
        {
            Rect rect = new(32, selectionRect.yMin, selectionRect.xMax, selectionRect.height);
            viewItem.displayName = gameObject.name.Substring(3).Trim();
            EditorGUI.DrawRect(rect, HighlightColor);
        }

        [MenuItem("Tools/Shalico/Hierarchy Enhancer/Toggle Highlight %H")]
        public static void ToggleHighlight()
        {
            if (Selection.gameObjects.Length == 0)
                return;

            Undo.RecordObjects(Selection.gameObjects, "Toggle Highlight");

            var gameObjects = Selection.gameObjects;
            foreach (var gameObject in gameObjects)
                if (IsHighlighted(gameObject))
                    gameObject.name = gameObject.name[3..].Trim();
                else
                    gameObject.name = "+++ " + gameObject.name;
            EditorApplication.RepaintHierarchyWindow();
        }

        [MenuItem("Tools/Shalico/Hierarchy Enhancer/Select All Highlighted %#H")]
        public static void SelectAllHighlighted()
        {
            GameObject[] arrayToSearch;
            if (Selection.gameObjects.Length > 0)
            {
                List<GameObject> list = new();
                list.AddRange(Selection.gameObjects.SelectMany(go => go.GetComponentsInChildren<Transform>())
                    .Select(t => t.gameObject));
                arrayToSearch = list.ToArray();
            }
            else
            {
                arrayToSearch =
                    Object.FindObjectsByType<GameObject>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            }

            var highlighted = arrayToSearch
                .Where(IsHighlighted).ToArray();
            Selection.objects = highlighted;
        }
    }
}