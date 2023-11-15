using System;
using System.Collections.Generic;
using System.Reflection;
using ShalicoAttributePack.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ShalicoEffect.Editor
{
    public class SerializeInterfaceReorderableList<TContainer, TInterface, TAddMenuAttribute>
        where TContainer : class
        where TInterface : class
        where TAddMenuAttribute : Attribute, IAddMenuAttribute
    {
        private readonly ReorderableList _reorderableList;
        private AddMenuItem[] _cachedMenuItems;

        public SerializeInterfaceReorderableList(SerializedProperty property, GUIContent label)
        {
            _reorderableList = new ReorderableList(property.serializedObject, property);
            _reorderableList.drawHeaderCallback += rect => { DrawHeader(rect, label); };
            _reorderableList.onAddDropdownCallback += (_, _) => OpenAddMenu();
            _reorderableList.drawElementCallback += DrawElement;
            _reorderableList.elementHeightCallback += GetElementHeight;
            _reorderableList.onMouseUpCallback += OnMouseUp;

            _reorderableList.drawElementBackgroundCallback = DrawBackground;
        }

        private void OnMouseUp(ReorderableList list)
        {
            if (Event.current.button == 1)
            {
                OpenListMenu();
                Event.current.Use();
            }
        }

        private void DrawHeader(Rect rect, GUIContent label)
        {
            var text = $"{typeof(TInterface).Name} ({label.text})";
            EditorGUI.LabelField(rect, text);
            if (OptionButton(rect)) OpenListMenu();
        }

        private bool OptionButton(Rect rect)
        {
            var optionsButtonRect = new Rect(rect.x + rect.width - EditorGUIUtility.singleLineHeight,
                rect.y,
                EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);
            var result = GUI.Button(optionsButtonRect, "⋮");


            return result;
        }

        private void DrawBackground(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (Event.current.type != EventType.Repaint)
                return;

            if (index % 2 == 0)
            {
                var color = new Color(0.1f, 0.1f, 0.1f, 0.2f);

                EditorGUI.DrawRect(rect, color);
            }

            if (isFocused)
                EditorGUI.DrawRect(rect, new Color(0.1f, 0.6f, 0.6f, 0.1f));
            else if (isActive)
                EditorGUI.DrawRect(rect, new Color(1.0f, 1.0f, 1.0f, 0.05f));
        }

        private void DrawCustomLabel(Rect rect, string text, Color32 color)
        {
            var colorRect = new Rect(rect.x, rect.y, 5, rect.height);
            EditorGUI.DrawRect(colorRect, color);

            var textRect = new Rect(rect.x + 14, rect.y, rect.width - 14, rect.height);
            var style = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold
            };
            EditorGUI.LabelField(textRect, text, style);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            var type = element.managedReferenceValue.GetType();
            var color = Color.white;
            var text = type.Name;

            if (type.GetCustomAttribute(typeof(CustomListLabelAttribute)) is CustomListLabelAttribute attribute)
            {
                color = attribute.Color;
                text = attribute.Text;
            }

            var labelRect = new Rect(rect.x + 14, rect.y, rect.width - 14,
                EditorGUIUtility.singleLineHeight);
            DrawCustomLabel(labelRect, $"{text} ({element.displayName})", color);

            EditorGUI.PropertyField(rect, element, GUIContent.none, true);

            if (OptionButton(rect)) OpenElementMenu(index);
        }


        private float GetElementHeight(int index)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            return EditorGUI.GetPropertyHeight(element, true);
        }

        private SerializedProperty GetNewElement()
        {
            var index = _reorderableList.serializedProperty.arraySize;
            _reorderableList.serializedProperty.arraySize++;
            _reorderableList.index = index;
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
            return element;
        }

        private void OpenAddMenu()
        {
            var menu = new GenericMenu();
            foreach (var menuItem in GetCachedMenuItems())
                menu.AddItem(new GUIContent(menuItem.Attribute.Path), false, () =>
                {
                    var instance = (TInterface)Activator.CreateInstance(menuItem.Type);
                    GetNewElement().managedReferenceValue = instance;
                    _reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
                });
            menu.ShowAsContext();
        }

        private void OpenListMenu()
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Copy List"), false, () =>
            {
                var list = (TContainer)_reorderableList.serializedProperty.TraceParentValue();
                EditorGUIUtility.systemCopyBuffer = JsonUtility.ToJson(list);
            });
            menu.AddItem(new GUIContent("Paste List"), false, () =>
            {
                var list = _reorderableList.serializedProperty.TraceParentValue();

                try
                {
                    JsonUtility.FromJsonOverwrite(EditorGUIUtility.systemCopyBuffer, list);
                    _reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
                }
                catch (ArgumentException)
                {
                    Debug.LogWarning("Cannot paste list, the serialized data is incompatible.");
                }
            });

            menu.AddSeparator("");

            menu.AddItem(new GUIContent("Clear All"), false, () =>
            {
                _reorderableList.serializedProperty.ClearArray();
                _reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
            });

            menu.AddSeparator("");

            menu.AddItem(new GUIContent("Paste Item"), false, () =>
            {
                try
                {
                    var json = EditorGUIUtility.systemCopyBuffer;

                    try
                    {
                        var obj = CopiedItem.FromJson(json);

                        var newElementProperty = GetNewElement();
                        newElementProperty.managedReferenceValue = obj;
                        newElementProperty.serializedObject.ApplyModifiedProperties();
                    }
                    catch (ArgumentException)
                    {
                        Debug.LogWarning("Cannot paste item, the serialized data is incompatible.");
                    }

                    _reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });

            menu.ShowAsContext();
        }

        private void OpenElementMenu(int index)
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Copy"), false, () =>
            {
                var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                var obj = (TInterface)element.managedReferenceValue;
                EditorGUIUtility.systemCopyBuffer = CopiedItem.ToJson(obj);
            });

            menu.AddItem(new GUIContent("Duplicate"), false, () =>
            {
                var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                var obj = (TInterface)element.managedReferenceValue;
                var newElementProperty = GetNewElement();
                newElementProperty.managedReferenceValue = obj;
                newElementProperty.serializedObject.ApplyModifiedProperties();
            });

            menu.AddItem(new GUIContent("Remove"), false, () =>
            {
                _reorderableList.serializedProperty.DeleteArrayElementAtIndex(index);
                _reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
            });

            menu.ShowAsContext();
        }

        private AddMenuItem[] GetCachedMenuItems()
        {
            if (_cachedMenuItems != null)
                return _cachedMenuItems;

            var classesWithCustomAttribute = new List<AddMenuItem>();

            foreach (var type in TypeCache.GetTypesWithAttribute(typeof(TAddMenuAttribute)))
            {
                if (!type.IsClass || type.IsAbstract || !typeof(TInterface).IsAssignableFrom(type))
                    continue;

                foreach (var addMenuAttribute in type.GetCustomAttributes(typeof(TAddMenuAttribute), false))
                    classesWithCustomAttribute
                        .Add(new AddMenuItem((IAddMenuAttribute)addMenuAttribute, type));
            }

            _cachedMenuItems = classesWithCustomAttribute.ToArray();

            return _cachedMenuItems;
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
            public readonly Type Type;
            public readonly IAddMenuAttribute Attribute;

            public AddMenuItem(IAddMenuAttribute attribute, Type type)
            {
                Attribute = attribute;
                Type = type;
            }
        }

        [Serializable]
        private class CopiedItem
        {
            [SerializeField] private string json;
            [SerializeField] private string typeName;

            private CopiedItem(TInterface item)
            {
                json = JsonUtility.ToJson(item);
                typeName = item.GetType().AssemblyQualifiedName;
            }

            public TInterface ToItem()
            {
                var type = Type.GetType(typeName);
                return JsonUtility.FromJson(json, type) as TInterface;
            }

            public static string ToJson(TInterface item)
            {
                return JsonUtility.ToJson(new CopiedItem(item));
            }

            public static TInterface FromJson(string json)
            {
                return JsonUtility.FromJson<CopiedItem>(json).ToItem();
            }
        }
    }
}