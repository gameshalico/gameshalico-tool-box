using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Shalico.ToolBox.Editor
{
    internal static class HierarchyHighlight
    {
        private static readonly Color s_color = new Color(0.5f, 0.5f, 0.0f, 0.5f);
        
        public static bool IsHighlighted(GameObject gameObject)
        {
            return gameObject.name.StartsWith("+++");
        }
        
        public static void Fill(TreeViewItem viewItem, GameObject gameObject, Rect selectionRect)
        {
            Rect rect = new Rect(32, selectionRect.yMin, selectionRect.xMax, selectionRect.height);
            viewItem.displayName = gameObject.name.Substring(3).Trim();
            EditorGUI.DrawRect(rect, s_color);
        }

        [MenuItem("Tools/Shalico/Hierarchy/Toggle Highlight %H")]
        private static void ToggleHighlight()
        {
            Undo.RecordObjects(Selection.gameObjects, "Toggle Highlight");

            GameObject[] gameObjects = Selection.gameObjects;
            foreach(GameObject gameObject in gameObjects)
            {
                if(IsHighlighted(gameObject))
                {
                    gameObject.name = gameObject.name.Substring(3).Trim();
                }
                else
                {
                    gameObject.name = "+++ " + gameObject.name;
                }
            }
            EditorApplication.RepaintHierarchyWindow();
        }

        [MenuItem("Tools/Shalico/Hierarchy/Select All Highlighted %#H")]
        private static void SelectAllHighlighted()
        {
            Undo.RecordObjects(Object.FindObjectsOfType<GameObject>(), "Select All Highlighted");

            GameObject[] gameObjects = Object.FindObjectsOfType<GameObject>();
            List<GameObject> highlightedGameObjects = new List<GameObject>();
            foreach(GameObject gameObject in gameObjects)
            {
                if(IsHighlighted(gameObject))
                {
                    highlightedGameObjects.Add(gameObject);
                }
            }
            Selection.objects = highlightedGameObjects.ToArray();
        }
    }
}