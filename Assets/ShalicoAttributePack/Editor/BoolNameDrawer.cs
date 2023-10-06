using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace AttributePack.Editor
{
    public abstract class BoolNameDrawer : PropertyDrawer
    {
        protected abstract void OnGUIWithBool(bool value, Rect position, SerializedProperty property, GUIContent label);

        private bool TryGetBool(SerializedProperty property, out bool value)
        {
            var boolNameAttribute = (BoolNameAttribute)attribute;

            var target = property.serializedObject.targetObject;

            var targetPropertyInfo = target.GetType()
                .GetProperty(boolNameAttribute.PropertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (targetPropertyInfo != null && targetPropertyInfo.GetValue(target) is bool propertyValue)
            {
                value = propertyValue;
                return true;
            }

            var targetFieldInfo = target.GetType()
                .GetField(boolNameAttribute.PropertyName, BindingFlags.Instance | BindingFlags.NonPublic);
            if (targetFieldInfo != null && targetFieldInfo.GetValue(target) is bool fieldValue)
            {
                value = fieldValue;
                return true;
            }


            value = false;
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (TryGetBool(property, out var value))
            {
                OnGUIWithBool(value, position, property, label);
            }
            else
            {
                Debug.LogWarning($"BoolNameAttribute: {((BoolNameAttribute)attribute).PropertyName} is not found.");
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}