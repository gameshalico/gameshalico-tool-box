using UnityEditor;
using UnityEngine;

namespace ShalicoAttributePack.Editor
{
    public abstract class ConditionDrawer : PropertyDrawer
    {
        protected abstract void OnGUIWithCondition(bool value, Rect position, SerializedProperty property,
            GUIContent label);

        private static bool TryGetValue(ConditionAttribute attribute, SerializedProperty property, out object value)
        {
            var target = property.GetParentObject();

            if (ReflectionUtility.TryFindFieldOrPropertyValue(target, attribute.ConditionName, out value))
                return true;


            value = false;
            return false;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var conditionAttribute = (ConditionAttribute)attribute;

            if (TryGetValue(conditionAttribute, property, out var value))
            {
                OnGUIWithCondition(Equals(value, conditionAttribute.Value), position, property, label);
            }
            else
            {
                Debug.LogWarning($"ConditionAttribute: {((ConditionAttribute)attribute).ConditionName} is not found.");
                EditorGUI.PropertyField(position, property, label);
            }
        }
    }
}