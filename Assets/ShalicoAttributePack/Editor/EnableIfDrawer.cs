using UnityEditor;
using UnityEngine;

namespace ShalicoAttributePack.Editor
{
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    public class EnableIfDrawer : ConditionDrawer
    {
        protected override void OnGUIWithCondition(bool value, Rect position, SerializedProperty property,
            GUIContent label)
        {
            GUI.enabled = value;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }
    }
}