using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShalicoAttributePack.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : ConditionDrawer
    {
        protected override void OnConditionChanged(bool value, VisualElement container, SerializedProperty property)
        {
            var propertyField = container.Q<PropertyField>();
            propertyField.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
        }

        protected override void OnGUIWithCondition(Rect position, SerializedProperty property, GUIContent label,
            bool value)
        {
            if (value) EditorGUI.PropertyField(position, property, label);
        }

        protected override float GetPropertyHeightWithCondition(SerializedProperty property, GUIContent label,
            bool value)
        {
            return value ? EditorGUI.GetPropertyHeight(property, label) : 0;
        }
    }
}