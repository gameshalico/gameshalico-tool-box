using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Shalico.ToolBox.Editor
{
    internal static class HierarchyRowStripe
    {
        private const int RowHeight = 16;
        private const int OffsetY = -4;
        private readonly static Color[] s_colors = new Color[] { new Color(0, 0, 0, 0.1f), new Color(0, 0, 0, 0) };

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