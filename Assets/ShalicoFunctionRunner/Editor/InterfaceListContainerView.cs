using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShalicoFunctionRunner.Editor
{
    public class InterfaceListContainerView<TContainer, TInterface, TAddMenuAttribute>
        where TContainer : class
        where TInterface : class
        where TAddMenuAttribute : Attribute, IAddMenuAttribute
    {
        private readonly string _propertyName;

        private readonly Dictionary<string, InterfaceReorderableList<TContainer, TInterface,
                TAddMenuAttribute>>
            _reorderableLists = new();

        public InterfaceListContainerView(string propertyName)
        {
            _propertyName = propertyName;
        }

        public void OnGUI(Rect position, SerializedProperty property,
            GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded, label);
            if (!property.isExpanded)
                return;

            using (new EditorGUI.PropertyScope(position, label, property))
            {
                if (!_reorderableLists.TryGetValue(property.propertyPath, out var list))
                {
                    list = new InterfaceReorderableList<TContainer, TInterface,
                        TAddMenuAttribute>(property.FindPropertyRelative(_propertyName), label);
                    _reorderableLists.Add(property.propertyPath, list);
                }

                var listRect = new Rect(position.x,
                    position.y + EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing,
                    position.width, EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
                list.Draw(listRect);
            }
        }

        public float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (property.isExpanded && _reorderableLists.TryGetValue(property.propertyPath, out var list))
                return list.GetHeight() + EditorGUIUtility.singleLineHeight +
                       EditorGUIUtility.standardVerticalSpacing * 2;

            return EditorGUIUtility.singleLineHeight;
        }
    }
}