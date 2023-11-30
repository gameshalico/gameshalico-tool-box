using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ShalicoAttributePack.Editor
{
    [CustomPropertyDrawer(typeof(SubclassSelector))]
    public class SubclassSelectorDrawer : PropertyDrawer
    {
        private (object value, Type fieldType) GetPropertyInfo(SerializedProperty property)
        {
            var value = property.TracePropertyValue();

            return (value, ReflectionUtility.GetFieldElementType(fieldInfo));
        }

        private void UpdateProperty(SerializedProperty property, int index, TypeCache.TypeCollection types)
        {
            var newValue = index == -1 ? null : Activator.CreateInstance(types[index]);
            property.managedReferenceValue = newValue;
            property.serializedObject.ApplyModifiedProperties();
        }

        private int PopupTypesMenu(Rect position, TypeCache.TypeCollection types, Type fieldType, int prevIndex,
            GUIContent label)
        {
            var names = types.Select(x => x.Name).Prepend("<null>").ToArray();

            var popupPosition = position;
            popupPosition.width -= 14;
            popupPosition.x += 14;
            popupPosition.height = EditorGUIUtility.singleLineHeight;
            var nextIndex = EditorGUI.Popup(popupPosition, label.text, prevIndex + 1,
                names) - 1;

            return nextIndex;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var (value, fieldType) = GetPropertyInfo(property);

            var types = TypeCache.GetTypesDerivedFrom(fieldType);

            var prevIndex = value == null ? -1 : types.IndexOf(value.GetType());
            var nextIndex = PopupTypesMenu(position, types, fieldType, prevIndex, label);

            if (prevIndex != nextIndex)
                UpdateProperty(property, nextIndex, types);

            EditorGUI.PropertyField(position, property, GUIContent.none, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}