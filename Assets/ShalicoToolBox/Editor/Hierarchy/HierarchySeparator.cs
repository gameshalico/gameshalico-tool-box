using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.IMGUI.Controls;
using UnityEditor;

namespace Shalico.ToolBox.Editor
{
    internal static class HierarchySeparator
    {
        private static readonly Color s_backgroundColor = new Color(0.15f, 0.15f, 0.15f);
        private static readonly Color s_textColor = new Color(1, 1, 1, 0.8f);

        public static bool IsSeparator(GameObject gameObject)
        {
            return gameObject.name.StartsWith("---");
        }

        public static void Draw(GameObject gameObject, Rect selectionRect)
        {
            Rect rect = new Rect(32, selectionRect.yMin, selectionRect.xMax, selectionRect.height);
            Rect textRect = new Rect(rect.xMin + 16, rect.yMin, rect.width - 16, rect.height);

            string labelText = gameObject.name.Substring(3).Trim();
            EditorGUI.DrawRect(rect, s_backgroundColor);
            
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