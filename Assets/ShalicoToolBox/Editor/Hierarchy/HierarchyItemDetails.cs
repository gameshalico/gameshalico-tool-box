using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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
            Tag,
            ActiveCheckbox
        }
        private static void DrawLabelOnSide(Rect rect, string label, Color color)
        {
            GUIStyle guiStyle = new(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleRight,
                normal = { textColor = color }
            };
            EditorGUI.LabelField(rect, label, guiStyle);
        }

        [MenuItem("Tools/Shalico/Hierarchy/Toggle Side View %T")]
        private static void ToggleSideView()
        {
            s_hierarchySideView = (HierarchySideView)(((int)s_hierarchySideView + 1) % 5);
            EditorApplication.RepaintHierarchyWindow();
        }

        public static void Draw(GameObject gameObject, Rect selectionRect)
        {
            switch (s_hierarchySideView)
            {
                case HierarchySideView.Component:
                    if (Event.current.type == EventType.Repaint)
                        HierarchyIcon.DrawIcons(selectionRect, gameObject);
                    break;
                case HierarchySideView.Layer:
                    if (Event.current.type == EventType.Repaint)
                    {
                        Color layerColor = gameObject.layer == 0 ? s_defaultColor : s_layerColors[(gameObject.layer - 1) % s_layerColors.Length];
                        DrawLabelOnSide(selectionRect, LayerMask.LayerToName(gameObject.layer), layerColor);
                    }
                    break;
                case HierarchySideView.Tag:
                    if (Event.current.type == EventType.Repaint)
                        DrawLabelOnSide(selectionRect, gameObject.tag, gameObject.CompareTag("Untagged") ? s_defaultColor : s_taggedColor);
                    break;
                case HierarchySideView.ActiveCheckbox:
                    HierarchyActiveCheckbox.Draw(gameObject, selectionRect);
                    break;
            }
        }
    }
}