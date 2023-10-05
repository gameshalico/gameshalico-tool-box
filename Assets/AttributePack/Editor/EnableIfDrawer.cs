using UnityEditor;
using UnityEngine;

namespace AttributePack.Editor
{
    [CustomPropertyDrawer(typeof(EnableIfAttribute))]
    public class EnableIfDrawer : BoolNameDrawer
    {
        protected override void OnGUIWithBool(bool value, Rect position, SerializedProperty property,
            GUIContent label)
        {
            GUI.enabled = value;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }
}