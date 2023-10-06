using UnityEditor;
using UnityEngine;

namespace AttributePack.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : BoolNameDrawer
    {
        protected override void OnGUIWithBool(bool value, Rect position, SerializedProperty property, GUIContent label)
        {
            if (value) EditorGUI.PropertyField(position, property, label);
        }
    }
}