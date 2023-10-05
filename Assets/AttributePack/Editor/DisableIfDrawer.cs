using UnityEditor;
using UnityEngine;

namespace AttributePack.Editor
{
    [CustomPropertyDrawer(typeof(DisableIfAttribute))]
    public class DisableIfDrawer : BoolNameDrawer
    {
        protected override void OnGUIWithBool(bool value, Rect position, SerializedProperty property,
            GUIContent label)
        {
            GUI.enabled = !value;
            EditorGUI.PropertyField(position, property, label);
            GUI.enabled = true;
        }
    }
}