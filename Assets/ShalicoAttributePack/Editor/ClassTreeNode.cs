using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using UnityEditor;

namespace ShalicoAttributePack.Editor
{
    public class ClassTreeNode
    {
        private static readonly string DefaultPath = "Scripts";
        private ClassTreeNode _child;
        private ClassTreeNode _next;

        private ClassTreeNode(string name)
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

        public static ClassTreeNode ConstructTypeTree(Type baseType)
        {
            var typeTreeRoot = new ClassTreeNode(baseType.Name);
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

        private void AddChild(ClassTreeNode child)
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
            if (!string.IsNullOrEmpty(attribute?.Path))
                return attribute.Path.Split("/");

            var hierarchy = type.Namespace?.Split('.').Prepend(DefaultPath).ToArray();
            if (hierarchy == null)
                return new[] { DefaultPath };
            return hierarchy;
        }

        private void AddTypeToClassTree(Type type)
        {
            var attribute = type.GetCustomAttribute<CustomDropdownPathAttribute>();

            var hierarchyPath = GetHierarchyPath(attribute, type);

            var nodeName = string.IsNullOrEmpty(attribute?.Name) ? type.Name : attribute.Name;

            var parent = this;
            foreach (var parentName in hierarchyPath) parent = parent.GetOrCreateChild(parentName);


            var node = new ClassTreeNode(nodeName)
            {
                Type = type
            };
            parent.AddChild(node);
        }

        private ClassTreeNode GetOrCreateChild(string name)
        {
            foreach (var node in Children())
                if (node.Name == name)
                    return node;

            var newNode = new ClassTreeNode(name);
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

        public IEnumerable<ClassTreeNode> Children()
        {
            for (var node = _child; node != null; node = node._next)
                yield return node;
        }
    }
}