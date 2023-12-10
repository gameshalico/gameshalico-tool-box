using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShalicoAttributePack.Editor
{
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    public class EnableIfDrawer : ConditionDrawer
    {
        protected override void OnConditionChanged(bool value, VisualElement container, SerializedProperty property)
        {
            var propertyField = container.Q<PropertyField>();
            propertyField.SetEnabled(value);
        }

        protected override void OnGUIWithCondition(Rect position, SerializedProperty property, GUIContent label,
            bool value)
        {
            EditorGUI.BeginDisabledGroup(!value);
            EditorGUI.PropertyField(position, property, label);
            EditorGUI.EndDisabledGroup();
        }

        protected override float GetPropertyHeightWithCondition(SerializedProperty property, GUIContent label,
            bool value)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}