using UnityEditor;
using UnityEngine;

namespace ShalicoAttributePack.Editor
{
    [CustomPropertyDrawer(typeof(ShowIfAttribute))]
    public class ShowIfDrawer : ConditionDrawer
    {
        private bool _isShow;
        private bool _needLayout;

        protected override void OnGUIWithCondition(bool value, Rect position, SerializedProperty property,
            GUIContent label)
        {
            _isShow = value;
            if (value) EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _isShow
                ? EditorGUI.GetPropertyHeight(property, label, true)
                : -EditorGUIUtility.standardVerticalSpacing;
        }
    }
}