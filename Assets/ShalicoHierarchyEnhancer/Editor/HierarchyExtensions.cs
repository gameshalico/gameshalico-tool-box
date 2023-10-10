using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace ShalicoHierarchyEnhancer.Editor
{
    [InitializeOnLoad]
    internal static class HierarchyExtensions
    {
        private static readonly PropertyInfo LastInteractedHierarchyWindowProperty;
        private static object _treeView;
        private static MethodInfo _findItem;

        static HierarchyExtensions()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;

            var sceneHierarchyWindowType = Type.GetType(
                "UnityEditor.SceneHierarchyWindow, UnityEditor.CoreModule");
            LastInteractedHierarchyWindowProperty =
                sceneHierarchyWindowType?.GetProperty("lastInteractedHierarchyWindow",
                    BindingFlags.Static | BindingFlags.Public);
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            if (_treeView == null)
                ExtractTreeView();

            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;

            if (gameObject)
            {
                if (Event.current.type == EventType.Repaint)
                {
                    HierarchyRowStripe.FillRow(selectionRect);
                    HierarchyIndentGuide.Fill(selectionRect);
                }

                if (HierarchySeparator.IsSeparator(gameObject))
                {
                    HierarchySeparator.Draw(gameObject, selectionRect);
                    return;
                }

                var item = FindItem(instanceID);
                if (item == null)
                {
                    ExtractTreeView();
                    return;
                }

                item.icon = HierarchyIcon.GetIcon(gameObject);

                if (HierarchyHighlight.IsHighlighted(gameObject))
                    HierarchyHighlight.Fill(item, gameObject, selectionRect);

                HierarchySideView.Draw(gameObject, selectionRect);
            }
        }


        private static TreeViewItem FindItem(int instanceID)
        {
            return (TreeViewItem)_findItem.Invoke(_treeView, new object[] { instanceID });
        }

        private static void ExtractTreeView()
        {
            var lastWindow = LastInteractedHierarchyWindowProperty
                .GetValue(null);
            var sceneHierarchy = lastWindow
                .GetType()
                .GetField("m_SceneHierarchy", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(lastWindow);

            _treeView = sceneHierarchy?.GetType()
                .GetField("m_TreeView", BindingFlags.Instance | BindingFlags.NonPublic)
                ?.GetValue(sceneHierarchy);

            _findItem = _treeView?.GetType()
                .GetMethod("FindItem", BindingFlags.Instance | BindingFlags.Public);
        }
    }
}