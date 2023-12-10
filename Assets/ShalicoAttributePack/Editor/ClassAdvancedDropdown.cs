using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ShalicoAttributePack.Editor
{
    public class ClassAdvancedDropdown : AdvancedDropdown
    {
        private static readonly Dictionary<Type, ClassTreeNode> s_typeTreeCache = new();
        private static readonly float MinimumWidth = EditorGUIUtility.singleLineHeight * 10;
        private readonly Type _baseType;
        private readonly Dictionary<int, Type> _types;
        public Action<Type> onSelectItem;

        public ClassAdvancedDropdown(Type baseType, AdvancedDropdownState state) : base(state)
        {
            _baseType = baseType;
            _types = new Dictionary<int, Type>();
            minimumSize = new Vector2(MinimumWidth, minimumSize.y);
        }

        private static ClassTreeNode GetCachedTypeTree(Type baseType)
        {
            if (s_typeTreeCache.TryGetValue(baseType, out var typeTree))
                return typeTree;

            typeTree = ClassTreeNode.ConstructTypeTree(baseType);
            s_typeTreeCache.Add(baseType, typeTree);
            return typeTree;
        }

        private AdvancedDropdownItem CreateHierarchyByTreeNode(ClassTreeNode node)
        {
            var item = new AdvancedDropdownItem(node.Name);

            foreach (var child in node.Children()) item.AddChild(CreateHierarchyByTreeNode(child));

            if (node.IsLeaf)
            {
                item.icon = GetIconByType(node.Type);
                if (node.Type != null || !_types.ContainsKey(item.id))
                    _types.TryAdd(item.id, node.Type);
            }

            return item;
        }

        private Texture2D GetIconByType(Type type)
        {
            if (type.GetCustomAttribute<IconAttribute>() is { } iconAttribute)
                return AssetDatabase.LoadAssetAtPath<Texture2D>(iconAttribute.path);
            return EditorGUIUtility.Load("cs Script Icon") as Texture2D;
        }

        private bool TryGetType(int id, out Type type)
        {
            return _types.TryGetValue(id, out type);
        }

        protected override AdvancedDropdownItem BuildRoot()
        {
            var typeTree = GetCachedTypeTree(_baseType);
            var root = new AdvancedDropdownItem(_baseType.Name);
            var nullItem = new AdvancedDropdownItem("<null>");
            root.AddChild(nullItem);
            foreach (var child in typeTree.Children()) root.AddChild(CreateHierarchyByTreeNode(child));
            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            if (TryGetType(item.id, out var type))
                onSelectItem?.Invoke(type);
            else
                onSelectItem?.Invoke(null);
        }
    }
}