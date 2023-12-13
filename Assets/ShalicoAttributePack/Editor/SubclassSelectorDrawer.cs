using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ShalicoAttributePack.Editor
{
    [CustomPropertyDrawer(typeof(SubclassSelector))]
    public class SubclassSelectorDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var (value, fieldType) = GetPropertyInfo(property);

            var buttonWidth = position.width - EditorGUIUtility.labelWidth;
            var buttonRect = new Rect(position)
            {
                x = position.xMax - buttonWidth,
                height = EditorGUIUtility.singleLineHeight,
                width = buttonWidth
            };
            if (GUI.Button(buttonRect, GetTypeName(value)))
                ShowDropdown(buttonRect, property, fieldType);

            if (value == null)
            {
                EditorGUI.LabelField(position, label);
                return;
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        private string GetTypeName(object value)
        {
            if (value == null)
                return SubclassAdvancedDropdown.NullTypeName;

            var type = value.GetType();
            if (type.GetCustomAttribute<CustomDropdownPathAttribute>() is { } pathAttribute &&
                !string.IsNullOrEmpty(pathAttribute.Name)) return pathAttribute.Name;

            return type.Name;
        }

        private void ShowDropdown(Rect position, SerializedProperty property, Type fieldType)
        {
            var dropdown = new SubclassAdvancedDropdown(fieldType, new AdvancedDropdownState(), true);
            dropdown.OnSelectItem += type =>
            {
                property.managedReferenceValue = type == null ? null : Activator.CreateInstance(type);
                property.serializedObject.ApplyModifiedProperties();
            };
            dropdown.Show(position);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        private (object value, Type fieldType) GetPropertyInfo(SerializedProperty property)
        {
            var value = property.TracePropertyValue();

            return (value, ReflectionUtility.GetFieldElementType(fieldInfo));
        }
    }
}