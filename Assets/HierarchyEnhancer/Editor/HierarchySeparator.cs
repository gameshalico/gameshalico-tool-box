using UnityEditor;
using UnityEngine;

namespace HierarchyEnhancer.Editor
{
    internal static class HierarchySeparator
    {
        private static readonly Color s_backgroundColor = new(0.15f, 0.15f, 0.15f);
        private static readonly Color s_textColor = new(1, 1, 1, 0.8f);

        public static bool IsSeparator(GameObject gameObject)
        {
            return gameObject.name.StartsWith("---");
        }

        public static void Draw(GameObject gameObject, Rect selectionRect)
        {
            Rect rect = new(32, selectionRect.yMin, selectionRect.xMax, selectionRect.height);
            Rect textRect = new(rect.xMin + 16, rect.yMin, rect.width - 16, rect.height);

            var labelText = gameObject.name.Substring(3).Trim();
            EditorGUI.DrawRect(rect, s_backgroundColor);

            GUIStyle style = new()
            {
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState
                {
                    textColor = s_textColor
                }
            };

            EditorGUI.LabelField(textRect, labelText, style);
        }

        [MenuItem("Tools/Shalico/Hierarchy/Toggle Separator %j")]
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