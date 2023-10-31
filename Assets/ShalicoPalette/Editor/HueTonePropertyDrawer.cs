using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ShalicoPalette.Editor
{
    [CustomPropertyDrawer(typeof(HueTone))]
    public class HueTonePropertyDrawer : PropertyDrawer
    {
        private static readonly string[] HueSymbolNames = Enum.GetNames(typeof(HueSymbol)).Zip(
            (HueSymbol[])Enum.GetValues(typeof(HueSymbol)), (name, value) => $"{(int)value:D2} : {name}"
        ).ToArray();

        private static readonly string[] ToneNames = Enum.GetNames(typeof(Tone));

        private void ScrollCheck(SerializedProperty hue, SerializedProperty tone, Rect hueRect, Rect toneRect)
        {
            if (Event.current.type == EventType.ScrollWheel)
                if (hueRect.Contains(Event.current.mousePosition))
                {
                    var hueIndex = (int)(HueSymbol)hue.enumValueIndex;
                    if (Event.current.delta.y > 0)
                    {
                        hueIndex++;
                        if (hueIndex >= HueSymbolNames.Length) hueIndex = 0;
                    }
                    else if (Event.current.delta.y < 0)
                    {
                        hueIndex--;
                        if (hueIndex < 0) hueIndex = HueSymbolNames.Length - 1;
                    }

                    hue.enumValueIndex = hueIndex;
                }
                else if (toneRect.Contains(Event.current.mousePosition))
                {
                    var toneIndex = (int)(Tone)tone.enumValueIndex;
                    if (Event.current.delta.y > 0)
                    {
                        toneIndex++;
                        if (toneIndex >= ToneNames.Length) toneIndex = 0;
                    }
                    else if (Event.current.delta.y < 0)
                    {
                        toneIndex--;
                        if (toneIndex < 0) toneIndex = ToneNames.Length - 1;
                    }

                    tone.enumValueIndex = toneIndex;
                }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var hueTone = (HueTone)fieldInfo.GetValue(property.serializedObject.targetObject);
            var hue = property.FindPropertyRelative("hue");
            var tone = property.FindPropertyRelative("tone");
            var hueIndex = (int)(HueSymbol)hue.enumValueIndex;
            var toneIndex = (int)(Tone)tone.enumValueIndex;

            if (hueIndex == -1)
            {
                hueIndex = 1;
                hue.enumValueIndex = hueIndex;
            }

            var color = Tones.GetColor((Tone)toneIndex, (HueSymbol)(hueIndex + 1));
            var rect = EditorGUI.PrefixLabel(position, label);
            var hueRect = new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, rect.height);
            var toneRect = new Rect(rect.x, rect.y, rect.width * 0.5f, rect.height);

            ScrollCheck(hue, tone, hueRect, toneRect);

            EditorGUI.BeginChangeCheck();
            hueIndex = EditorGUI.Popup(hueRect, hueIndex, HueSymbolNames);
            toneIndex = EditorGUI.Popup(toneRect, toneIndex, ToneNames);

            if (EditorGUI.EndChangeCheck())
            {
                hue.enumValueIndex = hueIndex;
                tone.enumValueIndex = toneIndex;
            }

            var colorRect = new Rect(rect.x, rect.y + rect.height, rect.width, rect.height);
            EditorGUI.DrawRect(colorRect, color);

            var colorLabelColor = ((Color)color).grayscale > 0.5f ? Color.black : Color.white;
            EditorGUI.LabelField(colorRect, hueTone.ToString(), new GUIStyle(
                EditorStyles.label)
            {
                normal = new GUIStyleState
                {
                    textColor = colorLabelColor
                },
                alignment = TextAnchor.MiddleCenter
            });

            EditorGUILayout.Space(EditorGUIUtility.singleLineHeight);

            EditorGUI.EndProperty();
        }
    }
}