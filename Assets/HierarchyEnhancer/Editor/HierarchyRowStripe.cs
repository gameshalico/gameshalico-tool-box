using UnityEditor;
using UnityEngine;

namespace HierarchyEnhancer.Editor
{
    internal static class HierarchyRowStripe
    {
        private const int RowHeight = 16;
        private const int OffsetY = -4;
        private static readonly Color[] s_colors = { new(0, 0, 0, 0.1f), new(0, 0, 0, 0) };

        public static void FillRow(Rect selectionRect)
        {
            var index = (int)(selectionRect.y + OffsetY) / RowHeight;
            var color = s_colors[index % s_colors.Length];

            var xMax = selectionRect.xMax;
            selectionRect.x = 0;
            selectionRect.xMax = xMax + 16;
            EditorGUI.DrawRect(selectionRect, color);
        }
    }
}