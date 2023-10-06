using UnityEditor;
using UnityEngine;

namespace HierarchyEnhancer.Editor
{
    internal static class HierarchySideView
    {
        private static SideType _hierarchySideView = SideType.None;
        private static readonly Color DefaultColor = new(1f, 1f, 1f, 0.5f);
        private static readonly Color TaggedColor = new(0.5f, 1f, 0.5f, 1f);

        private static readonly Color[] LayerColors =
        {
            new(1f, 0.5f, 0.5f, 1f),
            new(0.5f, 1f, 0.5f, 1f),
            new(0.5f, 0.5f, 1f, 1f),
            new(0.5f, 0.5f, 1f, 1f),
            new(1f, 1f, 0.5f, 1f),
            new(1f, 0.5f, 1f, 1f),
            new(0.5f, 1f, 1f, 1f)
        };

        private static void DrawLabelOnSide(Rect rect, string label, Color color)
        {
            GUIStyle guiStyle = new(EditorStyles.label)
            {
                alignment = TextAnchor.MiddleRight,
                normal = { textColor = color }
            };
            EditorGUI.LabelField(rect, label, guiStyle);
        }

        [MenuItem("Tools/Shalico/Hierarchy Enhancer/Toggle Side View %T")]
        private static void ToggleSideView()
        {
            _hierarchySideView = (SideType)(((int)_hierarchySideView + 1) % 5);
            EditorApplication.RepaintHierarchyWindow();
        }

        public static void Draw(GameObject gameObject, Rect selectionRect)
        {
            switch (_hierarchySideView)
            {
                case SideType.Component:
                    if (Event.current.type == EventType.Repaint)
                        HierarchyIcon.DrawIcons(selectionRect, gameObject);
                    break;
                case SideType.Layer:
                    if (Event.current.type == EventType.Repaint)
                    {
                        var layerColor = gameObject.layer == 0
                            ? DefaultColor
                            : LayerColors[(gameObject.layer - 1) % LayerColors.Length];
                        DrawLabelOnSide(selectionRect, LayerMask.LayerToName(gameObject.layer), layerColor);
                    }

                    break;
                case SideType.Tag:
                    if (Event.current.type == EventType.Repaint)
                        DrawLabelOnSide(selectionRect, gameObject.tag,
                            gameObject.CompareTag("Untagged") ? DefaultColor : TaggedColor);
                    break;
                case SideType.ActiveCheckbox:
                    HierarchyActiveCheckbox.Draw(gameObject, selectionRect);
                    break;
            }
        }

        private enum SideType
        {
            None,
            Component,
            Layer,
            Tag,
            ActiveCheckbox
        }
    }
}