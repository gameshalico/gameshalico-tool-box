using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace ShalicoAttributePack.Editor
{
    public abstract class ConditionDrawer : PropertyDrawer
    {
        protected abstract void OnConditionChanged(bool value, VisualElement container, SerializedProperty property);

        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var container = new VisualElement();
            var propertyField = new PropertyField(property);
            container.Add(propertyField);

            var conditionAttribute = (ConditionAttribute)attribute;

            void UpdateView()
            {
                var target = property.TraceParentValue();
                if (ReflectionUtility.TryFindFieldOrPropertyValue(target, conditionAttribute.ConditionName,
                        out var value))
                {
                    OnConditionChanged(Equals(value, conditionAttribute.Value), container, property);
                }
                else
                {
                    Debug.LogWarning(
                        $"ConditionAttribute: {((ConditionAttribute)attribute).ConditionName} is not found.");
                    OnConditionChanged(false, container, property);
                }
            }

            container.TrackSerializedObjectValue(property.serializedObject, _ => UpdateView());
            UpdateView();
            return container;
        }
    }
}