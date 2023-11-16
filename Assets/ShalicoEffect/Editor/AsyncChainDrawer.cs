using ShalicoEffect.FunctionRunners;
using UnityEditor;
using UnityEngine;

namespace ShalicoEffect.Editor
{
    [CustomPropertyDrawer(typeof(AsyncChainFunctionRunner))]
    public class AsyncChainDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var asyncChain = property.FindPropertyRelative("asyncChain");
            EditorGUI.PropertyField(position, asyncChain, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var asyncChain = property.FindPropertyRelative("asyncChain");
            return EditorGUI.GetPropertyHeight(asyncChain, label);
        }
    }
}