using Shalico.ToolBox.Editor;
using UnityEditor;
using UnityEngine;

namespace Shalico.ToolBox
{
    [CustomPropertyDrawer(typeof(ValueRange))]
    public class ValueRangeDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            int indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // 表示欄を2つに分ける
            float width = (position.width - 10) / 2;
            Rect minRect = new(position.x, position.y, width, position.height);
            Rect maxRect = new(position.x + width + 10, position.y, width, position.height);

            // ラベルなしで表示
            EditorGUI.PropertyField(minRect, property.FindPropertyRelative("min"), GUIContent.none);
            EditorGUI.PropertyField(maxRect, property.FindPropertyRelative("max"), GUIContent.none);

            EditorGUI.indentLevel = indent;
            EditorGUI.EndProperty();
        }
    }
}