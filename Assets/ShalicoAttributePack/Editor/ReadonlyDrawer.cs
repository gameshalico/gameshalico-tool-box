using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace ShalicoAttributePack.Editor
{
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    public class ReadonlyDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            var element = new PropertyField(property);
            element.SetEnabled(false);
            return element;
        }
    }
}