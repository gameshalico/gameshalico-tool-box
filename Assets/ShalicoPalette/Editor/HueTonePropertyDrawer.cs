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

        private static readonly string[] ToneNames = Enum.GetNames(typeof(Tone)).Zip(
            (Tone[])Enum.GetValues(typeof(Tone)),
            (name, value) => $"{Tones.GetTone(value).ToneSymbol} : {name}"
        ).ToArray();


        private void ScrollCheck(SerializedProperty enumProperty, Rect rect, int enumIndex, int enumCount,
            int increment)
        {
            if (Event.current.type == EventType.ScrollWheel)
                if (rect.Contains(Event.current.mousePosition))
                {
                    if (Event.current.delta.y < 0) increment = -increment;
                    enumIndex += increment;
                    if (enumIndex >= enumCount) enumIndex = 0;
                    else if (enumIndex < 0) enumIndex = enumCount - 1;

                    enumProperty.enumValueIndex = enumIndex;
                    Event.current.Use();
                }
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            var hue = property.FindPropertyRelative("hue");
            var tone = property.FindPropertyRelative("tone");
            var hueIndex = (int)(HueSymbol)hue.enumValueIndex;
            var toneIndex = (int)(Tone)tone.enumValueIndex;

            if (hueIndex == -1)
            {
                hueIndex = 1;
                hue.enumValueIndex = hueIndex;
            }

            var toneData = Tones.GetTone((Tone)toneIndex);

            var color = Tones.GetColor((Tone)toneIndex, (HueSymbol)(hueIndex + 1));
            var rect = EditorGUI.PrefixLabel(position, label);
            var hueRect = new Rect(rect.x + rect.width * 0.5f, rect.y, rect.width * 0.5f, rect.height);
            var toneRect = new Rect(rect.x, rect.y, rect.width * 0.5f, rect.height);

            if (toneData.IsNeutral) toneRect.width = rect.width;

            EditorGUI.BeginChangeCheck();
            if (!toneData.IsNeutral)
            {
                if (toneData.IsVivid)
                {
                    hueIndex = EditorGUI.Popup(hueRect, hueIndex, HueSymbolNames);
                    ScrollCheck(hue, hueRect, hueIndex, HueSymbolNames.Length, 1);
                }
                else
                {
                    hueIndex = EditorGUI.Popup(hueRect, hueIndex / 2,
                        HueSymbolNames.Where((name, index) => index % 2 == 1).ToArray()) * 2;
                    ScrollCheck(hue, hueRect, hueIndex, HueSymbolNames.Length, 2);
                }
            }

            toneIndex = EditorGUI.Popup(toneRect, toneIndex, ToneNames);
            ScrollCheck(tone, toneRect, toneIndex, ToneNames.Length, 1);

            if (EditorGUI.EndChangeCheck())
            {
                hue.enumValueIndex = hueIndex;
                tone.enumValueIndex = toneIndex;
            }

            var colorRect = new Rect(rect.x, rect.y + rect.height, rect.width, rect.height);
            EditorGUI.DrawRect(colorRect, color);

            var colorLabelColor = ((Color)color).grayscale > 0.5f ? Color.black : Color.white;
            EditorGUI.LabelField(colorRect, new HueTone(hueIndex + 1, (Tone)toneIndex).ToString(), new GUIStyle(
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