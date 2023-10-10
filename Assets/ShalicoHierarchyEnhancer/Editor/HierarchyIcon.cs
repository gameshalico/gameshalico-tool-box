using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace ShalicoHierarchyEnhancer.Editor
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
                .Select(GetIconTexture)
                .Where(t => t != null)
                .Distinct();

            var x = rect.xMax - 16;
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
            if (components.Length == 1 || components[1] == null)
                return components[0];

            if (components[1] is CanvasRenderer &&
                components.Length > 2)
                return components[2];

            return components[1];
        }

        public static Texture2D GetIconTexture(Component component)
        {
            if (component == null)
                return null;
            var type = component.GetType();

            if (s_iconCache.TryGetValue(type, out var cachedIcon))
                return cachedIcon;

            var icon = AssetPreview.GetMiniThumbnail(component);
            s_iconCache.Add(type, icon);
            return icon;
        }
    }
}