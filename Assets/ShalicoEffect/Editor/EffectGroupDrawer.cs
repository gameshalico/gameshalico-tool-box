using System;
using System.Collections.Generic;
using ShalicoAttributePack.Editor;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace ShalicoEffect.Editor
{
    [CustomPropertyDrawer(typeof(EffectGroup))]
    public class EffectGroupDrawer : PropertyDrawer
    {
        private static (Type type, AddEffectMenuAttribute attribute)[] s_cachedEffectMenuItems;


        private readonly Dictionary<string, ReorderableList> _reorderableLists = new();

        private static (Type type, AddEffectMenuAttribute attribute)[] GetEffectMenuItems()
        {
            if (s_cachedEffectMenuItems != null)
                return s_cachedEffectMenuItems;

            var classesWithCustomAttribute = new List<(Type type, AddEffectMenuAttribute attribute)>();
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            foreach (var type in assembly.GetTypes())
                if (type.IsClass && !type.IsAbstract && typeof(IEffect).IsAssignableFrom(type))
                    foreach (var addEffectMenu in type.GetCustomAttributes(typeof(AddEffectMenuAttribute), false))
                        classesWithCustomAttribute.Add((type, (AddEffectMenuAttribute)addEffectMenu));

            s_cachedEffectMenuItems = classesWithCustomAttribute.ToArray();

            return s_cachedEffectMenuItems;
        }

        private void ShowAddEffectMenu(SerializedProperty property)
        {
            var effectMenuItems = GetEffectMenuItems();

            var group = (EffectGroup)property.FindNestedPropertyValue();

            var menu = new GenericMenu();

            foreach (var menuItem in effectMenuItems)
            {
                var path = menuItem.attribute.Path;

                menu.AddItem(new GUIContent(path), false, () =>
                {
                    Undo.RecordObject(property.serializedObject.targetObject, $"Add {menuItem.type.Name}");

                    var effectType = menuItem.type;
                    var effectInstance = (IEffect)Activator.CreateInstance(effectType);
                    group.AddEffect(effectInstance);
                });
            }

            menu.ShowAsContext();
        }

        private void DrawCustomLabel(Rect rect, string text, Color32 color)
        {
            var colorRect = new Rect(rect.x, rect.y, 5, rect.height);
            EditorGUI.DrawRect(colorRect, color);

            var textRect = new Rect(rect.x + 14, rect.y, rect.width - 10, rect.height);
            var style = new GUIStyle(EditorStyles.label)
            {
                fontStyle = FontStyle.Bold
            };
            EditorGUI.LabelField(textRect, text, style);
        }

        private void CopyGroup(SerializedProperty property)
        {
            var group = (EffectGroup)property.FindNestedPropertyValue();
            EditorGUIUtility.systemCopyBuffer = JsonUtility.ToJson(group);
        }

        private void PasteGroup(SerializedProperty property)
        {
            var group = (EffectGroup)property.FindNestedPropertyValue();
            try
            {
                JsonUtility.FromJsonOverwrite(EditorGUIUtility.systemCopyBuffer, group);
            }
            catch (ArgumentException)
            {
            }
        }

        private void ClearGroup(SerializedProperty property)
        {
            var group = (EffectGroup)property.FindNestedPropertyValue();
            group.ClearEffects();
        }

        private void CopyEffect(SerializedProperty property)
        {
            var effect = (IEffect)property.FindNestedPropertyValue();
            EditorGUIUtility.systemCopyBuffer = EffectUtility.SerializeEffect(effect);
        }

        private void RemoveEffect(SerializedProperty property)
        {
            var group = (EffectGroup)property.FindParentObject();
            group.RemoveEffect((IEffect)property.FindNestedPropertyValue());
        }

        private void DuplicateEffect(SerializedProperty property)
        {
            var effect = (IEffect)property.FindNestedPropertyValue();

            Undo.RecordObject(property.serializedObject.targetObject, $"Duplicate {effect.GetType().Name}");

            var group = (EffectGroup)property.FindParentObject();
            EffectUtility.TryDeserializeEffect(EffectUtility.SerializeEffect(effect), out var duplicate);
            group.AddEffect(duplicate);
        }

        private void DrawEffectProperty(Rect rect, SerializedProperty property)
        {
            var labelRect = new Rect(rect.x + 14, rect.y, rect.width,
                EditorGUIUtility.singleLineHeight);

            var type = property.FindNestedPropertyValue().GetType();

            var customHeaderAttribute =
                (EffectCustomHeaderAttribute)Attribute.GetCustomAttribute(type,
                    typeof(EffectCustomHeaderAttribute));
            if (customHeaderAttribute != null)
                DrawCustomLabel(labelRect, customHeaderAttribute.Text + $" ({property.displayName})",
                    customHeaderAttribute.Color);
            else
                DrawCustomLabel(labelRect, type.Name + $" ({property.displayName})", Color.white);


            var optionsButtonRect = new Rect(rect.x + rect.width - EditorGUIUtility.singleLineHeight, rect.y,
                EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);

            if (GUI.Button(optionsButtonRect, "⋮"))
            {
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Copy"), false, () => CopyEffect(property));
                menu.AddItem(new GUIContent("Duplicate"), false, () => DuplicateEffect(property));
                menu.AddItem(new GUIContent("Remove"), false, () => RemoveEffect(property));
                menu.ShowAsContext();
            }

            EditorGUI.PropertyField(rect, property, GUIContent.none, true);
        }

        private ReorderableList GetOrCreateReorderableList(SerializedProperty property, GUIContent label)
        {
            if (_reorderableLists.TryGetValue(property.propertyPath, out var list))
                return list;

            var listProperty = property.FindPropertyRelative("_effects");
            var reorderableList =
                new ReorderableList(listProperty.serializedObject, listProperty, true, true, true, true)
                {
                    drawHeaderCallback = rect =>
                    {
                        var text = $"Effect Group ({label.text})";
                        EditorGUI.LabelField(rect, text);

                        var optionsButtonRect = new Rect(rect.x + rect.width - EditorGUIUtility.singleLineHeight,
                            rect.y,
                            EditorGUIUtility.singleLineHeight, EditorGUIUtility.singleLineHeight);

                        if (GUI.Button(optionsButtonRect, "⋮"))
                        {
                            var menu = new GenericMenu();
                            menu.AddItem(new GUIContent("Copy List"), false, () => CopyGroup(property));
                            menu.AddItem(new GUIContent("Paste List"), false, () => PasteGroup(property));
                            menu.AddItem(new GUIContent("Clear All"), false, () => ClearGroup(property));
                            menu.ShowAsContext();
                        }
                    },
                    drawElementCallback = (rect, index, isActive, isFocused) =>
                    {
                        var element = listProperty.GetArrayElementAtIndex(index);

                        DrawEffectProperty(rect, element);
                    },
                    elementHeightCallback = index =>
                    {
                        var element = listProperty.GetArrayElementAtIndex(index);
                        return EditorGUI.GetPropertyHeight(element, true);
                    },
                    onAddCallback = _ => ShowAddEffectMenu(property),
                    drawFooterCallback = rect =>
                    {
                        var buttonsRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                        // add remove paste buttons
                        var addRect = new Rect(buttonsRect.x + buttonsRect.width - 40, buttonsRect.y, 20,
                            buttonsRect.height);
                        var removeRect = new Rect(buttonsRect.x + buttonsRect.width - 20, buttonsRect.y, 20,
                            buttonsRect.height);
                        var pasteRect = new Rect(buttonsRect.x, buttonsRect.y, 60,
                            buttonsRect.height);
                        if (GUI.Button(addRect, "+"))
                            ShowAddEffectMenu(property);

                        if (_reorderableLists.TryGetValue(property.propertyPath, out var reorderableList))
                        {
                            GUI.enabled = reorderableList.index >= 0 &&
                                          reorderableList.index < reorderableList.count;
                            if (GUI.Button(removeRect, "-"))
                            {
                                var effectGroup = (EffectGroup)property.FindNestedPropertyValue();
                                Undo.RecordObject(property.serializedObject.targetObject, "Remove");
                                effectGroup.RemoveEffectAt(reorderableList.index);
                            }
                        }

                        GUI.enabled = EditorGUIUtility.systemCopyBuffer.Length > 0;
                        if (GUI.Button(pasteRect, "Paste"))
                            if (EffectUtility.TryDeserializeEffect(EditorGUIUtility.systemCopyBuffer, out var effect))
                            {
                                var effectGroup = (EffectGroup)property.FindNestedPropertyValue();
                                Undo.RecordObject(property.serializedObject.targetObject, "Paste");
                                effectGroup.AddEffect(effect);
                            }

                        GUI.enabled = true;
                    }
                };
            _reorderableLists.Add(property.propertyPath, reorderableList);

            return reorderableList;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded, label);
            if (!property.isExpanded)
                return;

            // indent
            position.x += 14;
            position.width -= 14;

            var list = GetOrCreateReorderableList(property, label);
            EditorGUI.BeginProperty(position, label, property);

            var listRect = new Rect(position.x, position.y + EditorGUIUtility.singleLineHeight,
                position.width, EditorGUIUtility.singleLineHeight);
            using (new EditorGUI.PropertyScope(position, label, property))
            {
                list.DoList(listRect);
            }

            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            if (_reorderableLists.TryGetValue(property.propertyPath, out var list))
                return list.GetHeight() + EditorGUIUtility.singleLineHeight;
            return EditorGUIUtility.singleLineHeight;
        }
    }
}