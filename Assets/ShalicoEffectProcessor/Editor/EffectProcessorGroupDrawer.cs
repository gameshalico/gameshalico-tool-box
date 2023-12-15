using UnityEditor;
using UnityEngine;

namespace ShalicoEffectProcessor.Editor
{
    [CustomPropertyDrawer(typeof(EffectPlayer))]
    public class EffectProcessorGroupDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var effectProcessorProperty = property.FindPropertyRelative("effectProcessor");

            EditorGUI.PropertyField(position, effectProcessorProperty, label);
        }


        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property.FindPropertyRelative("effectProcessor"));
        }
    }
}