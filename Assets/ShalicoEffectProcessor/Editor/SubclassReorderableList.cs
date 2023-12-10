using System;
using System.Reflection;
using ShalicoAttributePack.Editor;
using ShalicoColorPalette;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEditorInternal;
using UnityEngine;

namespace ShalicoEffectProcessor.Editor
{
    public class SubclassReorderableList<TContainer, TBase>
        where TContainer : class
        where TBase : class
    {
        private readonly ReorderableList _reorderableList;

        public SubclassReorderableList(SerializedProperty property, GUIContent label)
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
            var text = $"{typeof(TBase).Name} ({label.text})";
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
                EditorGUI.DrawRect(rect, new Color(0.6f, 0.6f, 0.1f, 0.05f));
        }

        private void DrawCustomLabel(Rect rect, string labelText, int index, string displayName, Color32 color)
        {
            var colorRect = new Rect(rect.x, rect.y, 5, rect.height);
            EditorGUI.DrawRect(colorRect, color);

            var labelTextStyle = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold
            };
            var label = new GUIContent(labelText);
            var labelRect = new Rect(rect.x + 14, rect.y, labelTextStyle.CalcSize(label).x, rect.height);
            EditorGUI.LabelField(labelRect, label, labelTextStyle);

            var subTextStyle = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Normal,
                normal =
                {
                    textColor = EditorStyles.label.normal.textColor.Alpha(0x80)
                }
            };

            var subText = new GUIContent($" ({index} : {displayName})");
            var subTextRect = new Rect(labelRect.xMax + 5, rect.y, subTextStyle.CalcSize(subText).x, rect.height);
            EditorGUI.LabelField(subTextRect, subText, subTextStyle);
        }

        private void DrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);

            if (element.managedReferenceValue == null)
            {
                var style = new GUIStyle(EditorStyles.label)
                {
                    normal =
                    {
                        textColor = Color.magenta
                    }
                };

                EditorGUI.LabelField(rect, $"<Null / Missing type referenced : {element.managedReferenceId}>", style);
                return;
            }

            var type = element.managedReferenceValue.GetType();
            var color = Color.white;
            var nameText = type.Name;

            if (type.GetCustomAttribute(typeof(CustomListLabelAttribute)) is CustomListLabelAttribute attribute)
            {
                color = attribute.Color;
                nameText = attribute.Text;
            }

            var labelRect = new Rect(rect.x + 14, rect.y, rect.width - 14,
                EditorGUIUtility.singleLineHeight);
            DrawCustomLabel(labelRect, nameText, index, element.displayName, color);

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
            var dropdown = new ClassAdvancedDropdown(typeof(TBase), new AdvancedDropdownState());
            dropdown.onSelectItem += type =>
            {
                GetNewElement().managedReferenceValue = type == null ? null : Activator.CreateInstance(type);
                ApplyChanges();
            };
            var mousePosition = Event.current.mousePosition;
            mousePosition.x -= 200;
            dropdown.Show(new Rect(mousePosition, Vector2.zero));
        }

        private void ApplyChanges()
        {
            _reorderableList.serializedProperty.serializedObject.ApplyModifiedProperties();
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
                    ApplyChanges();
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
                ApplyChanges();
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

                    ApplyChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            });

            menu.AddSeparator("");

            if (SerializationUtility.HasManagedReferencesWithMissingTypes(_reorderableList.serializedProperty
                    .serializedObject.targetObject))
                menu.AddItem(new GUIContent("Remove Missing References"), false, () =>
                {
                    // Remove missing references

                    for (var i = 0; i < _reorderableList.serializedProperty.arraySize; i++)
                    {
                        var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(i);
                        if (element.managedReferenceValue == null)
                        {
                            _reorderableList.serializedProperty.DeleteArrayElementAtIndex(i);
                            i--;
                        }
                    }

                    SerializationUtility.ClearAllManagedReferencesWithMissingTypes(_reorderableList.serializedProperty
                        .serializedObject.targetObject);

                    ApplyChanges();
                });
            else
                menu.AddDisabledItem(new GUIContent("Remove Missing References"));

            menu.ShowAsContext();
        }

        private void OpenElementMenu(int index)
        {
            var menu = new GenericMenu();

            menu.AddItem(new GUIContent("Copy"), false, () =>
            {
                var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                var obj = (TBase)element.managedReferenceValue;
                EditorGUIUtility.systemCopyBuffer = CopiedItem.ToJson(obj);
            });

            menu.AddItem(new GUIContent("Duplicate"), false, () =>
            {
                var element = _reorderableList.serializedProperty.GetArrayElementAtIndex(index);
                var obj = CopiedItem.ToJson((TBase)element.managedReferenceValue);
                var newElementProperty = GetNewElement();
                newElementProperty.managedReferenceValue = CopiedItem.FromJson(obj);
                newElementProperty.serializedObject.ApplyModifiedProperties();
            });

            menu.AddItem(new GUIContent("Remove"), false, () =>
            {
                _reorderableList.serializedProperty.DeleteArrayElementAtIndex(index);
                ApplyChanges();
            });

            menu.ShowAsContext();
        }


        public void Draw(Rect position)
        {
            _reorderableList.DoList(position);
        }

        public float GetHeight()
        {
            return _reorderableList.GetHeight();
        }

        [Serializable]
        private class CopiedItem
        {
            [SerializeField] private string json;
            [SerializeField] private string typeName;

            private CopiedItem(TBase item)
            {
                json = JsonUtility.ToJson(item);
                typeName = item.GetType().AssemblyQualifiedName;
            }

            public TBase ToItem()
            {
                var type = Type.GetType(typeName);
                return JsonUtility.FromJson(json, type) as TBase;
            }

            public static string ToJson(TBase item)
            {
                return JsonUtility.ToJson(new CopiedItem(item));
            }

            public static TBase FromJson(string json)
            {
                return JsonUtility.FromJson<CopiedItem>(json).ToItem();
            }
        }
    }
}