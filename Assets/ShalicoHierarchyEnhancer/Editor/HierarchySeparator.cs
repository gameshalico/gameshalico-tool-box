using UnityEditor;
using UnityEngine;

namespace ShalicoHierarchyEnhancer.Editor
{
    internal static class HierarchySeparator
    {
        private const int LeftMargin = 32 + 14 * 2;
        private static readonly Color BackgroundColor = new(0.1f, 0.1f, 0.1f);
        private static readonly Color TextColor = new(1, 1, 1, 0.8f);

        public static bool IsSeparator(GameObject gameObject)
        {
            return gameObject.name.StartsWith("---");
        }

        public static void Draw(GameObject gameObject, Rect selectionRect)
        {
            Rect rect = new(LeftMargin, selectionRect.yMin, selectionRect.xMax, selectionRect.height);
            Rect textRect = new(LeftMargin, rect.yMin, rect.width - 32, rect.height);

            var labelText = gameObject.name.Substring(3).Trim();
            EditorGUI.DrawRect(rect, BackgroundColor);

            GUIStyle style = new()
            {
                alignment = TextAnchor.MiddleCenter,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState
                {
                    textColor = TextColor
                }
            };

            EditorGUI.LabelField(textRect, labelText, style);
        }

        [MenuItem("Tools/Shalico/Hierarchy Enhancer/Toggle Separator %j")]
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