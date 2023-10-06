using UnityEditor;
using UnityEngine;

namespace HierarchyEnhancer.Editor
{
    internal static class HierarchyActiveCheckbox
    {
        public static void Draw(GameObject gameObject, Rect selectionRect)
        {
            Rect rect = new(selectionRect.xMax - 16, selectionRect.yMin, 16, selectionRect.height);
            EditorGUI.BeginChangeCheck();
            var active = EditorGUI.Toggle(rect, gameObject.activeSelf);
            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(gameObject, "Toggle Active");
                gameObject.SetActive(active);
            }
        }
    }
}