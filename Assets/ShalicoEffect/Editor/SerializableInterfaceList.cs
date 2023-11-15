using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ShalicoEffect.Editor
{
    public class SerializableInterfaceList<TInterface, TAddMenuAttribute>
        where TAddMenuAttribute : Attribute, IAddMenuAttribute
    {
        private static AddMenuItem[] s_cachedMenuItems;
        private readonly ReorderableList _reorderableList;

        public SerializableInterfaceList(SerializedProperty property, GUIContent label)
        {
            _reorderableList = new ReorderableList(property.serializedObject, property);
            _reorderableList.drawHeaderCallback += rect => { DrawHeader(rect, label); };
            _reorderableList.onAddCallback += _ => OpenAddMenu();
            _reorderableList.drawElementCallback += DrawElement;
            _reorderableList.elementHeightCallback += GetElementHeight;
        }

        private void DrawHeader(Rect rect, GUIContent label)
        {
            var text = $"{typeof(TInterface).Name} ({label.text})";
            EditorGUI.LabelField(rect, text);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);

            EditorGUI.PropertyField(rect, element, true);
        }

        private float GetElementHeight(int index)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(element, true);
        }

        private void OpenAddMenu()
        {
            var menu = new GenericMenu();
            foreach (var menuItem in GetCachedMenuItems())
                menu.AddItem(new GUIContent(menuItem.Attribute.Path), false, () =>
                {
                    var index = _reorderableList.serializedProperty.arraySize;
                    _reorderableList.serializedProperty.arraySize++;
                    _reorderableList.index = index;
                    var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                    element.managedReferenceValue = Activator.CreateInstance(menuItem.Type);
                    _reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
                });
            menu.ShowAsContext();
        }

        private static AddMenuItem[] GetCachedMenuItems()
        {
            if (s_cachedMenuItems != null)
                return s_cachedMenuItems;

            var classesWithCustomAttribute = new List<AddMenuItem>();

            foreach (var type in TypeCache.GetTypesWithAttribute(typeof(TAddMenuAttribute)))
            {
                if (!type.IsClass || type.IsAbstract || !typeof(TInterface).IsAssignableFrom(type))
                    continue;

                foreach (var addMenuAttribute in type.GetCustomAttributes(typeof(TAddMenuAttribute), false))
                    classesWithCustomAttribute
                        .Add(new AddMenuItem((TAddMenuAttribute)addMenuAttribute, type));
            }

            s_cachedMenuItems = classesWithCustomAttribute.ToArray();

            return s_cachedMenuItems;
        }

        public void Draw(Rect position)
        {
            _reorderableList.DoList(position);
        }

        public float GetHeight()
        {
            return _reorderableList.GetHeight();
        }

        private class AddMenuItem
        {
            public readonly TAddMenuAttribute Attribute;
            public readonly Type Type;

            public AddMenuItem(TAddMenuAttribute attribute, Type type)
            {
                Attribute = attribute;
                Type = type;
            }
        }
    }
}