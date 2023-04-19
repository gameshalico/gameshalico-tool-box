using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;
using System;
using UnityEditor.IMGUI.Controls;

namespace Shalico.ToolBox.Hierarchy
{
    internal static class HierarchyIcon
    {
        private static readonly Dictionary<Type, Texture2D> s_iconCache = new Dictionary<Type, Texture2D>();
        public static Texture2D GetIcon(GameObject gameObject)
        {
            var target = GetIconTarget(gameObject);
            return GetIconTexture(target);
        }

        private static Component GetIconTarget(GameObject gameObject)
        {
            var components = gameObject.GetComponents<Component>();
            if(components.Length == 1)
                return components[0];
            
            if (components[1] is CanvasRenderer &&
                components.Length > 2)
            {
                return components[2];
            }

            return components[1];
        }

        private static Texture2D GetIconTexture(Component component)
        {
            var type = component.GetType();
            if (s_iconCache.ContainsKey(type))
                return s_iconCache[type];

            var icon = EditorGUIUtility.ObjectContent(component, type).image as Texture2D;
            s_iconCache.Add(type, icon);
            return icon;
        }
    }
}