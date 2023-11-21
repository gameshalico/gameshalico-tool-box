using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ShalicoColorPalette.Editor
{
    [CustomPropertyDrawer(typeof(IndexedColor))]
    public class IndexedColorPropertyDrawer : PropertyDrawer
    {
        private void ScrollCheck(SerializedProperty intProperty, Rect rect, int index, int count,
            int increment)
        {
            if (Event.current.type == EventType.ScrollWheel)
                if (rect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.delta.y < 0) increment = -increment;
                    index += increment;
                    if (index >= count) index = 0;
                    else if (index < 0) index = count - 1;

                    intProperty.intValue = index;
                    Event.current.Use();
                }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var palette = PaletteSettings.DefaultPalette;
            if (palette == null) EditorGUI.LabelField(position, label, new GUIContent("Please set default palette."));

            var colors = palette.GetAllColorData();

            var colorIndex = property.FindPropertyRelative("colorIndex");

            var rect = EditorGUI.PrefixLabel(position, label);

            var popupRect = new Rect(rect.x, rect.y, rect.width * 0.5f, rect.height);
            var colorRect = new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, rect.height);
            EditorGUI.BeginChangeCheck();
            var newIndex = EditorGUI.Popup(popupRect, colorIndex.intValue,
                colors.Select(colorData => colorData.ColorName).ToArray());

            ScrollCheck(colorIndex, popupRect, newIndex, colors.Length, 1);
            if (EditorGUI.EndChangeCheck()) colorIndex.intValue = newIndex;

            EditorGUI.DrawRect(colorRect, colors[colorIndex.intValue].Color);
        }
    }
}