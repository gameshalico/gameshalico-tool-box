using ShalicoEffectProcessor.EffectProcessors;
using UnityEditor;
using UnityEngine;

namespace ShalicoEffectProcessor.Editor
{
    [CustomPropertyDrawer(typeof(EffectExecutorProcessor))]
    public class EffectExecutorProcessorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded, label);
            if (!property.isExpanded)
                return;

            var synchronizeProperty = property.FindPropertyRelative("synchronize");
            var effectGroupProperty = property.FindPropertyRelative("effectGroup");

            position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            var synchronizeRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            EditorGUI.PropertyField(synchronizeRect, synchronizeProperty);
            EditorGUI.PropertyField(position, effectGroupProperty);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            var synchronizeProperty = property.FindPropertyRelative("synchronize");
            var effectGroupProperty = property.FindPropertyRelative("effectGroup");

            return EditorGUI.GetPropertyHeight(synchronizeProperty) + EditorGUI.GetPropertyHeight(effectGroupProperty);
        }
    }
}