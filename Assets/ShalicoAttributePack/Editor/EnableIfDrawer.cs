using UnityEditor;
using UnityEditor.UIElements;
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
    }
}