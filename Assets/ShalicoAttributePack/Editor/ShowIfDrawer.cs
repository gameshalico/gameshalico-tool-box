using UnityEditor;
using UnityEditor.UIElements;
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
    }
}