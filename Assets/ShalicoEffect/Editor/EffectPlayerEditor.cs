using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace ShalicoEffect.Editor
{
    [CustomEditor(typeof(EffectPlayer))]
    public class EffectPlayerEditor : UnityEditor.Editor
    {
        private static (Type type, AddEffectMenuAttribute attribute)[] s_cachedEffectMenuItems;

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

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            if (GUILayout.Button("Add Effect"))
            {
                var attributes = GetEffectMenuItems();

                var menu = new GenericMenu();
                foreach (var attribute in attributes)
                {
                    var path = attribute.attribute.Path;

                    menu.AddItem(new GUIContent(path), false, () =>
                    {
                        Undo.RecordObject(serializedObject.targetObject, $"Add {attribute.type.Name}");
                        var effectPlayer = (EffectPlayer)serializedObject.targetObject;
                        var effectType = attribute.type;
                        var effectInstance = (IEffect)Activator.CreateInstance(effectType);
                        effectPlayer.AddEffect(effectInstance);
                        Debug.Log($"Added {effectType.Name}");
                    });
                }

                menu.ShowAsContext();
            }

            if (GUILayout.Button("Play Effects"))
            {
                var effectPlayer = (EffectPlayer)serializedObject.targetObject;
                effectPlayer.PlayEffects(default);
            }
        }
    }
}