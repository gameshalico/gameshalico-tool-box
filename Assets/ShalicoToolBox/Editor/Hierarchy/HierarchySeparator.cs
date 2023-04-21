using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;

namespace Shalico.ToolBox.Editor
{
    internal static class HierarchySeparator
    {
        private static readonly Color[] s_colors = new Color[]
        {
            new Color(0.15f, 0.15f, 0.15f),
            new Color(0.5f, 0.5f, 0.0f),
            new Color(0.0f, 0.5f, 0.0f),
            new Color(0.0f, 0.5f, 0.5f),
            new Color(0.0f, 0.0f, 0.5f),
            new Color(0.5f, 0.0f, 0.5f),
            new Color(0.5f, 0.0f, 0.0f),
        };
        private static readonly Color s_textColor = new Color(1, 1, 1, 0.8f);

        public static bool IsSeparator(GameObject gameObject)
        {
            return gameObject.name.StartsWith("---");
        }

        public static void Draw(GameObject gameObject, Rect selectionRect)
        {
            Rect rect = new Rect(32, selectionRect.yMin, selectionRect.xMax, selectionRect.height);
            Rect textRect = new Rect(rect.xMin + 14, rect.yMin, rect.width - 14, rect.height);

            string labelText = gameObject.name.Substring(3);
            int colorIndex = 0;
            if(labelText.Length > 0 && char.IsNumber(labelText[0]))
            {
                Debug.Log(labelText);
                colorIndex = (int)char.GetNumericValue(labelText[0]);
                labelText = labelText.Substring(1);
            }
            labelText.Trim();

            EditorGUI.DrawRect(rect, s_colors[colorIndex % s_colors.Length]);
            
            GUIStyle style = new GUIStyle()
            {
                alignment = TextAnchor.MiddleLeft,
                fontStyle = FontStyle.Bold,
                normal = new GUIStyleState() {
                    textColor = s_textColor
                }
            };

            EditorGUI.LabelField(textRect, labelText, style);
        }
    }
}