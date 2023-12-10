using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace ShalicoAttributePack.Editor
{
    public class ClassDropdownTreeNode
    {
        private static readonly string DefaultPath = "Scripts";
        private ClassDropdownTreeNode _child;
        private ClassDropdownTreeNode _next;

        private ClassDropdownTreeNode(string name)
        {
            Name = name;
        }

        public bool IsLeaf => _child == null;

        public string Name { get; private set; }
        public Type Type { get; private set; }


        private static bool IsTypeInvalid(Type type)
        {
            return type.IsAbstract || type.IsGenericType || type.IsInterface;
        }

        public static ClassDropdownTreeNode ConstructTypeTree(Type baseType)
        {
            var typeTreeRoot = new ClassDropdownTreeNode(baseType.Name);
            foreach (var type in TypeCache.GetTypesDerivedFrom(baseType))
            {
                if (IsTypeInvalid(type))
                    continue;

                typeTreeRoot.AddTypeToClassTree(type);
            }

            foreach (var child in typeTreeRoot.Children())
            {
                var separator = "/";
                if (child.Name.Equals(DefaultPath)) separator = ".";
                foreach (var grandchild in child.Children())
                    grandchild.MergeSingleBranches(separator);
            }

            return typeTreeRoot;
        }

        public override string ToString()
        {
            return RecursiveToString(0);
        }

        private string RecursiveToString(int depth)
        {
            var sb = new StringBuilder();
            sb.Append(' ', depth * 4);
            sb.AppendLine(Name);
            foreach (var child in Children()) sb.Append(child.RecursiveToString(depth + 1));
            return sb.ToString();
        }

        private void AddChild(ClassDropdownTreeNode child)
        {
            if (_child == null)
            {
                _child = child;
                return;
            }

            foreach (var node in Children())
                if (node._next == null)
                {
                    node._next = child;
                    return;
                }
        }

        private static string[] GetHierarchyPath(CustomDropdownPathAttribute attribute, Type type)
        {
            if (attribute != null)
            {
                if (string.IsNullOrEmpty(attribute.Path))
                    return null;
                return attribute.Path.Split("/");
            }

            var hierarchy = type.Namespace?.Split('.')
                .Prepend(DefaultPath).Append(type.Name).ToArray();
            if (hierarchy == null)
                return new[] { DefaultPath, type.Name };
            return hierarchy;
        }

        private void AddTypeToClassTree(Type type)
        {
            var attribute = type.GetCustomAttribute<CustomDropdownPathAttribute>();

            var hierarchyPath = GetHierarchyPath(attribute, type);
            if (hierarchyPath == null)
                return;

            var nodeName = hierarchyPath.Last();

            var parent = this;
            foreach (var parentName in hierarchyPath.Take(hierarchyPath.Length - 1))
                parent = parent.GetOrCreateChild(parentName);


            var node = new ClassDropdownTreeNode(nodeName)
            {
                Type = type
            };
            parent.AddChild(node);
        }

        private ClassDropdownTreeNode GetOrCreateChild(string name)
        {
            foreach (var node in Children())
                if (node.Name == name)
                    return node;

            var newNode = new ClassDropdownTreeNode(name);
            AddChild(newNode);
            return newNode;
        }

        private void MergeSingleBranch(string separator)
        {
            do
            {
                var target = _child;
                if (target == null)
                    return;
                // 複数の子ノードがあるならマージしない
                if (target._next != null)
                    return;
                // 葉ノードならマージしない
                if (target._child == null)
                    return;

                Name += $"{separator}{target.Name}";
                _child = target._child;
            } while (true);
        }

        private void MergeSingleBranches(string separator)
        {
            MergeSingleBranch(separator);
            foreach (var child in Children()) child.MergeSingleBranches(separator);
        }

        public IEnumerable<ClassDropdownTreeNode> Children()
        {
            for (var node = _child; node != null; node = node._next)
                yield return node;
        }
    }
}