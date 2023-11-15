using System.Collections.Generic;
using ShalicoEffect.FunctionRunners;
using UnityEditor;
using UnityEngine;

namespace ShalicoEffect.Editor
{
    [CustomPropertyDrawer(typeof(FunctionRunnerChain))]
    public class FunctionRunnerChainDrawer : PropertyDrawer
    {
        private readonly Dictionary<string, SerializableInterfaceList<IFunctionRunner, AddFunctionRunnerMenuAttribute>>
            _runnersLists = new();

        public override void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded, label);
            if (!property.isExpanded)
                return;

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                position.x += 14;
                position.width -= 14;
                var listRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight,
                    position.width, EditorGUIUtility.singleLineHeight);

                if (_runnersLists.TryGetValue(property.propertyPath, out var list))
                {
                    list.Draw(listRect);
                    return;
                }

                _runnersLists.Add(property.propertyPath,
                    new SerializableInterfaceList<IFunctionRunner, AddFunctionRunnerMenuAttribute>(
                        property.FindPropertyRelative("_runners"), label));

                _runnersLists[property.propertyPath].Draw(listRect);
            }
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded && _runnersLists.TryGetValue(property.propertyPath, out var list))
                return list.GetHeight() + EditorGUIUtility.singleLineHeight;

            return EditorGUIUtility.singleLineHeight;
        }
    }
}