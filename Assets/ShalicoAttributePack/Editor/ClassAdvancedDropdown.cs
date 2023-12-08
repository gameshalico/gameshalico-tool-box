using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;

namespace ShalicoAttributePack.Editor
{
    public class ClassAdvancedDropdown : AdvancedDropdown
    {
        private readonly Type _baseType;
        private readonly Dictionary<int, Type> _types;

        public ClassAdvancedDropdown(Type baseType, AdvancedDropdownState state) : base(state)
        {
            _baseType = baseType;
            _types = new Dictionary<int, Type>();
        }

        private static bool IsTypeInvalid(Type type)
        {
            return type.IsAbstract || type.IsGenericType || type.IsInterface;
        }

        private static string[] GetHierarchyArray(Type type)
        {
            if (type.GetCustomAttribute<CustomDropdownPathAttribute>() is { } customPathAttribute)
                return customPathAttribute.Path.Split('.');

            var hierarchy = type.Namespace?.Split('.');
            return hierarchy;
        }

        private static AdvancedDropdownItem GetOrCreateHierarchyItem(AdvancedDropdownItem root, string[] hierarchy)
        {
            var parent = root;
            if (hierarchy != null)
                foreach (var name in hierarchy)
                {
                    var child = parent.children.FirstOrDefault(x => x.name == name);
                    if (child == null)
                    {
                        child = new AdvancedDropdownItem(name);
                        parent.AddChild(child);
                    }

                    parent = child;
                }

            return parent;
        }

        private void RegisterItem(AdvancedDropdownItem parent, Type type)
        {
            var node = new AdvancedDropdownItem(type.Name);
            parent.AddChild(node);
            _types.Add(node.id, type);
        }

        public bool TryGetType(int id, out Type type)
        {
            return _types.TryGetValue(id, out type);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var root = new AdvancedDropdownItem(_baseType.Name);
            foreach (var type in TypeCache.GetTypesDerivedFrom(_baseType))
            {
                if (IsTypeInvalid(type))
                    continue;

                var hierarchy = GetHierarchyArray(type);
                var parent = GetOrCreateHierarchyItem(root, hierarchy);
                RegisterItem(parent, type);
            }

            return root;
        }
    }
}