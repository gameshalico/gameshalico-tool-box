using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Unity.VisualScripting.YamlDotNet.Core.Tokens;

namespace Shalico.ToolBox.Editor
{
    internal static class HierarchyItemDetails
    {
        private static HierarchySideView s_hierarchySideView = HierarchySideView.None;
        private static readonly Color s_defaultColor = new(1f, 1f, 1f, 0.5f);
        private static readonly Color s_taggedColor = new(0.5f, 1f, 0.5f, 1f);
        private static readonly Color[] s_layerColors = new Color[]
        {
            new Color(1f, 0.5f, 0.5f, 1f),
            new Color(0.5f, 1f, 0.5f, 1f),
            new Color(0.5f, 0.5f, 1f, 1f),
            new Color(0.5f, 0.5f, 1f, 1f),
            new Color(1f, 1f, 0.5f, 1f),
            new Color(1f, 0.5f, 1f, 1f),
            new Color(0.5f, 1f, 1f, 1f)
        };

        private enum HierarchySideView
        {
            None,
            Component,
            Layer,
            Tag
        }
        private static void DrawLabelOnSide(Rect rect, string label, Color color)
        {
            GUIStyle guiStyle = new(EditorStyles.label) {
                alignment = TextAnchor.MiddleRight,
                normal = { textColor = color }
            };
            EditorGUI.LabelField(rect, label, guiStyle);
        }

        [MenuItem("Tools/Shalico/Hierarchy/Toggle Side View %H")]
        private static void ToggleSideView()
        {
            s_hierarchySideView = (HierarchySideView)(((int)s_hierarchySideView + 1) % 4);
            EditorApplication.RepaintHierarchyWindow();
        }

        public static void Draw(GameObject gameObject, Rect selectionRect)
        {
            switch(s_hierarchySideView)
            {
                case HierarchySideView.Component:
                    HierarchyIcon.DrawIcons(selectionRect, gameObject);
                    break;
                case HierarchySideView.Layer:
                    Color layerColor = gameObject.layer == 0 ? s_defaultColor : s_layerColors[gameObject.layer - 1 % s_layerColors.Length];
                    DrawLabelOnSide(selectionRect, LayerMask.LayerToName(gameObject.layer), layerColor);
                    break;
                case HierarchySideView.Tag:
                    DrawLabelOnSide(selectionRect, gameObject.tag, gameObject.CompareTag("Untagged") ? s_defaultColor : s_taggedColor);
                    break;
            }
        }
    }
}
