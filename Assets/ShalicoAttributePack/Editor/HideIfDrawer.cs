﻿using ShalicoAttributePack.Runtime;
using UnityEditor;
using UnityEngine;

namespace ShalicoAttributePack.Editor
{
    [CustomPropertyDrawer(typeof(HideIfAttribute))]
    public class HideIfDrawer : BoolNameDrawer
    {
        protected override void OnGUIWithBool(bool value, Rect position, SerializedProperty property, GUIContent label)
        {
            if (!value) EditorGUI.PropertyField(position, property, label);
        }
    }
}