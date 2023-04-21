using System;
using System.Reflection;
using log4net.Core;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;

namespace Shalico.ToolBox.Editor
{
    [InitializeOnLoad]
    internal static class HierarchyExtension
    {
        private static readonly PropertyInfo s_lastInteractedHierarchyWindowProperty;
        private static object s_treeView;
        private static MethodInfo s_findItem;
        private static MethodInfo s_initTree;

        static HierarchyExtension()
        {
            EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
        
            Type sceneHierarchyWindowType = Type.GetType(
                "UnityEditor.SceneHierarchyWindow, UnityEditor.CoreModule, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null");
            s_lastInteractedHierarchyWindowProperty = sceneHierarchyWindowType.GetProperty("lastInteractedHierarchyWindow", BindingFlags.Static | BindingFlags.Public);
            
        }

        private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
        {
            if (s_treeView == null)
                ExtractTreeView();

            var gameObject = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
            
            if(gameObject)
            {
                if (HierarchySeparator.IsSeparator(gameObject))
                {
                    HierarchySeparator.Draw(gameObject, selectionRect);
                    return;
                }

                TreeViewItem item = FindItem(instanceID);
                item.icon = HierarchyIcon.GetIcon(gameObject);

                if (HierarchyHighlight.IsHighlighted(gameObject))
                    HierarchyHighlight.Fill(item, gameObject, selectionRect);

                HierarchyRowStripe.FillRow(selectionRect);
                HierarchyIndentGuide.Fill(selectionRect);
                HierarchyItemDetails.Draw(gameObject, selectionRect);
            }
        }


        private static TreeViewItem FindItem(int instanceID)
        {
            return (TreeViewItem)s_findItem.Invoke(s_treeView, new object[] { instanceID });
        }

        private static void ExtractTreeView()
        {
            var lastWindow = s_lastInteractedHierarchyWindowProperty
                .GetValue(null);
            var sceneHierarchy = lastWindow
                .GetType()
                .GetField("m_SceneHierarchy", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(lastWindow);

            s_treeView = sceneHierarchy
                .GetType()
                .GetField("m_TreeView", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(sceneHierarchy);

            s_findItem = s_treeView
                .GetType()
                .GetMethod("FindItem", BindingFlags.Instance | BindingFlags.Public);

            s_initTree = sceneHierarchy
                .GetType()
                .GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }

}
