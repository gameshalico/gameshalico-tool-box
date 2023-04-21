using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace Shalico.ToolBox.Editor
{
    internal static class HierarchyHighlight
    {
        private static readonly Color s_color = new Color(0.5f, 0.5f, 0.0f, 0.5f);
        
        public static bool IsHighlighted(GameObject gameObject)
        {
            return gameObject.name.StartsWith("+++");
        }
        
        public static void Fill(TreeViewItem viewItem, GameObject gameObject, Rect selectionRect)
        {
            Rect rect = new Rect(32, selectionRect.yMin, selectionRect.xMax, selectionRect.height);
            viewItem.displayName = gameObject.name.Substring(3).Trim();
            EditorGUI.DrawRect(rect, s_color);
        }
    }
}