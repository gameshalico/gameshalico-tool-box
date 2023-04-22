using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Shalico.ToolBox.Editor
{
    internal static class HierarchyIcon
    {
        private static readonly Dictionary<Type, Texture2D> s_iconCache = new();

        public static Texture2D GetIcon(GameObject gameObject)
        {
            var target = GetIconTarget(gameObject);
            return GetIconTexture(target);
        }

        public static void DrawIcons(Rect rect, GameObject gameObject)
        {
            var icons = gameObject.GetComponents<Component>()
                .Where(c => c is not Transform)
                .Reverse()
                .Select(c => GetIconTexture(c))
                .Where(t => t != null)
                .Distinct();

            float x = rect.xMax - 16;
            foreach (var icon in icons)
            {
                var iconRect = new Rect(x, rect.y, 16, 16);
                GUI.DrawTexture(iconRect, icon);
                x -= 16;
            }
        }

        private static Component GetIconTarget(GameObject gameObject)
        {
            var components = gameObject.GetComponents<Component>();
            if (components.Length == 1)
                return components[0];

            if (components[1] is CanvasRenderer &&
                components.Length > 2)
            {
                return components[2];
            }

            return components[1];
        }

        public static Texture2D GetIconTexture(Component component)
        {
            var type = component.GetType();

            if (s_iconCache.ContainsKey(type))
                return s_iconCache[type];

            var icon = AssetPreview.GetMiniThumbnail(component);
            s_iconCache.Add(type, icon);
            return icon;
        }
    }
}