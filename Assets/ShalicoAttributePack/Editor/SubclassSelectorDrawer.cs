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
        private const float OptionButtonSize = 14;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var (value, fieldType) = GetPropertyInfo(property);

            var buttonWidth = position.width - EditorGUIUtility.labelWidth;
            var buttonRect = new Rect(position)
            {
                x = position.xMax - buttonWidth,
                height = EditorGUIUtility.singleLineHeight,
                width = buttonWidth - EditorGUIUtility.singleLineHeight
            };
            if (GUI.Button(buttonRect, GetTypeName(value)))
                ShowDropdown(buttonRect, property, fieldType);

            if (OptionButton(position))
                OpenCopyAndPasteMenu(property);

            if (value == null)
            {
                EditorGUI.LabelField(position, label);
                return;
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        private bool OptionButton(Rect rect)
        {
            var optionsButtonRect = new Rect(rect.x + rect.width - EditorGUIUtility.singleLineHeight,
                rect.y + EditorGUIUtility.singleLineHeight / 2 - OptionButtonSize / 2,
                OptionButtonSize, OptionButtonSize);

            var image = EditorGUIUtility.IconContent("_Menu");
            var result = GUI.Button(optionsButtonRect, image, EditorStyles.iconButton);

            return result;
        }

        private void OpenCopyAndPasteMenu(SerializedProperty serializedProperty)
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Copy"), false,
                () =>
                {
                    EditorGUIUtility.systemCopyBuffer = ValueWithType.ToJson(serializedProperty.managedReferenceValue);
                });

            if (ValueWithType.TryParse(EditorGUIUtility.systemCopyBuffer, out var value))
            {
                var valueType = value.GetType();
                var pasteText = $"Paste as {valueType.Name}";

                var fieldType = fieldInfo.FieldType;
                if (fieldType.HasElementType)
                    fieldType = fieldType.GetElementType();

                if (fieldType?.IsAssignableFrom(valueType) ?? false)
                    menu.AddItem(new GUIContent(pasteText), false, () =>
                    {
                        serializedProperty.managedReferenceValue = value;
                        serializedProperty.serializedObject.ApplyModifiedProperties();
                    });
                else
                    menu.AddDisabledItem(new GUIContent(pasteText));
            }
            else
            {
                menu.AddDisabledItem(new GUIContent("Paste"));
            }

            menu.ShowAsContext();
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