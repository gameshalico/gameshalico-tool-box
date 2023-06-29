using UnityEditor;
using UnityEngine;

namespace HierarchyEnhancer.Editor
{
    internal static class HierarchyIndentGuide
    {
        private const float Alpha = 0.1f;

        private static readonly Color[] s_colors =
        {
            new(1.0f, 1.0f, 0.5f),
            new(0.5f, 1, 0.5f),
            new(1, 0.5f, 0.5f),
            new(0.5f, 0.5f, 1.0f)
        };

        public static void Fill(Rect selectionRect)
        {
            var xMax = selectionRect.xMin - 14;
            var depth = 0;
            for (var ix = 32; ix < xMax; ix += 14)
            {
                var color = s_colors[depth % s_colors.Length];
                color.a = Alpha;
                EditorGUI.DrawRect(new Rect(ix, selectionRect.y, 14, 16), color);
                depth++;
            }
        }
    }
}