using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ShalicoAttributePack.Editor
{
    public class SubclassAdvancedDropdown : AdvancedDropdown
    {
        public static readonly string NullTypeName = "null";

        private static readonly Dictionary<Type, TypeDropdownTreeNode> TypeTreeCache = new();
        private static readonly Vector2 DefaultMinimumSize = new(200, 300);
        private readonly Type _baseType;
        private readonly bool _nullable;
        private readonly Dictionary<int, Type> _types;
        public Action<Type> OnSelectItem;

        public SubclassAdvancedDropdown(Type baseType, Vector2 minimumSize, AdvancedDropdownState state = default,
            bool nullable = false) :
            base(state)
        {
            _baseType = baseType;
            _types = new Dictionary<int, Type>();
            _nullable = nullable;
            this.minimumSize = minimumSize;
        }

        public SubclassAdvancedDropdown(Type baseType, AdvancedDropdownState state = default, bool nullable = false) :
            this(baseType, DefaultMinimumSize, state, nullable)
        {
        }

        private static TypeDropdownTreeNode GetCachedTypeTree(Type baseType)
        {
            if (TypeTreeCache.TryGetValue(baseType, out var typeTree))
                return typeTree;

            typeTree = TypeDropdownTreeNode.ConstructTypeTree(baseType);
            TypeTreeCache.Add(baseType, typeTree);
            return typeTree;
        }

        private AdvancedDropdownItem CreateHierarchyByTreeNode(TypeDropdownTreeNode treeNode)
        {
            var item = new AdvancedDropdownItem(treeNode.Name);

            foreach (var child in treeNode.Children()) item.AddChild(CreateHierarchyByTreeNode(child));

            if (treeNode.IsLeaf)
            {
                item.icon = GetIconByType(treeNode.Type);
                if (treeNode.Type != null || !_types.ContainsKey(item.id))
                    _types.TryAdd(item.id, treeNode.Type);
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
            if (_nullable)
            {
                var nullItem = new AdvancedDropdownItem(NullTypeName);
                root.AddChild(nullItem);
            }

            foreach (var child in typeTree.Children()) root.AddChild(CreateHierarchyByTreeNode(child));
            return root;
        }

        protected override void ItemSelected(AdvancedDropdownItem item)
        {
            OnSelectItem?.Invoke(TryGetType(item.id, out var type) ? type : null);
        }
    }
}