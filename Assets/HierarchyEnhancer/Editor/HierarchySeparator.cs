using UnityEditor;
using UnityEngine;

namespace HierarchyEnhancer.Editor
{
    internal static class HierarchySeparator
    {
        private static readonly Color BackgroundColor = new(0.5f, 0.5f, 0.5f);
        private static readonly Color TextColor = new(1, 1, 1, 0.8f);

        public static bool IsSeparator(GameObject gameObject)
        {
            return gameObject.name.StartsWith("---");
        }

        public static void Draw(GameObject gameObject, Rect selectionRect)
        {
            Rect rect = new(32, selectionRect.yMin, selectionRect.xMax, selectionRect.height);
            Rect textRect = new(rect.xMin + 16, rect.yMin, rect.width - 16, rect.height);

            var labelText = gameObject.name.Substring(3).Trim();
            EditorGUI.DrawRect(rect, BackgroundColor);

            GUIStyle style = new()
            {
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState
                {
                    textColor = TextColor
                }
            };

            EditorGUI.LabelField(textRect, labelText, style);
        }

        [MenuItem("Tools/Hierarchy Enhancer/Toggle Separator %j")]
        private static void ToggleSeparator()
        {
            Undo.RecordObjects(Selection.gameObjects, "Toggle Separator");

            var gameObjects = Selection.gameObjects;
            foreach (var gameObject in gameObjects)
                if (IsSeparator(gameObject))
                    gameObject.name = gameObject.name.Substring(3).Trim();
                else
                    gameObject.name = "--- " + gameObject.name;
            EditorApplication.RepaintHierarchyWindow();
        }
    }
}