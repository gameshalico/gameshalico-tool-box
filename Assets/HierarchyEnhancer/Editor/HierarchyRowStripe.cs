using UnityEditor;
using UnityEngine;

namespace HierarchyEnhancer.Editor
{
    internal static class HierarchyRowStripe
    {
        private const int RowHeight = 16;
        private const int OffsetY = -4;
        private static readonly Color[] RowColors = { new(0, 0, 0, 0.1f), new(0, 0, 0, 0) };

        public static void FillRow(Rect selectionRect)
        {
            var index = (int)(selectionRect.y + OffsetY) / RowHeight;
            var color = RowColors[index % RowColors.Length];

            var xMax = selectionRect.xMax;
            selectionRect.x = 0;
            selectionRect.xMax = xMax + 16;
            EditorGUI.DrawRect(selectionRect, color);
        }
    }
}