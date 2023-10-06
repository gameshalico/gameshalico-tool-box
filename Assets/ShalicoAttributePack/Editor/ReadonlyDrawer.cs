using UnityEditor;
using UnityEngine;

namespace AttributePack.Editor
{
    [CustomPropertyDrawer(typeof(ReadonlyAttribute))]
    public class ReadonlyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            GUI.enabled = false;
            EditorGUI.PropertyField(position, property, label, true);
            GUI.enabled = true;
        }
    }
}