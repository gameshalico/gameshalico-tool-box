using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace Shalico.ToolBox.Editor
{
    internal static class HierarchyIndentGuide
    {
        private static readonly Color[] s_colors = new Color[]
        {
            new Color(1.0f, 1.0f, 0.5f),
            new Color(0.5f, 1, 0.5f),
            new Color(1, 0.5f, 0.5f),
            new Color(0.5f, 0.5f, 1.0f),
        };
        private const float Alpha = 0.1f;

        public static void Fill(Rect selectionRect)
        {
            float xMax = selectionRect.xMin - 14;
            int depth = 0;
            for(int ix = 32; ix < xMax; ix += 14)
            {
                Color color = s_colors[depth % s_colors.Length];
                color.a = Alpha;
                EditorGUI.DrawRect(new Rect(ix, selectionRect.y, 14, 16), color);
                depth++;
            }
        }
    }
}
