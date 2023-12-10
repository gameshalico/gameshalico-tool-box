using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShalicoAttributePack.Editor
{
    public abstract class ConditionDrawer : PropertyDrawer
    {
        protected abstract void OnConditionChanged(bool value, VisualElement container, SerializedProperty property);

        protected abstract void OnGUIWithCondition(Rect position, SerializedProperty property, GUIContent label,
            bool value);

        protected abstract float GetPropertyHeightWithCondition(SerializedProperty property, GUIContent label,
            bool value);

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var propertyField = new PropertyField(property);
            container.Add(propertyField);

            void UpdateView()
            {
                TryGetConditionValue(property, out var value);
                OnConditionChanged(value, container, property);
            }

            container.TrackSerializedObjectValue(property.serializedObject, _ => UpdateView());
            UpdateView();
            return container;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            TryGetConditionValue(property, out var value);
            OnGUIWithCondition(position, property, label, value);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            TryGetConditionValue(property, out var value);
            return GetPropertyHeightWithCondition(property, label, value);
        }

        private bool TryGetConditionValue(SerializedProperty property, out bool value)
        {
            var conditionAttribute = (ConditionAttribute)attribute;
            var target = property.TraceParentValue();
            if (ReflectionUtility.TryFindFieldOrPropertyValue(target, conditionAttribute.ConditionName,
                    out var conditionValue))
            {
                value = Equals(conditionValue, conditionAttribute.Value);
                return true;
            }

            Debug.LogWarning(
                $"ConditionAttribute: {((ConditionAttribute)attribute).ConditionName} is not found.");
            value = false;
            return false;
        }
    }
}