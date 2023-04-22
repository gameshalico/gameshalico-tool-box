using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Shalico.ToolBox.Editor
{

    public static class SelectionShortcuts
    {
        [MenuItem("Tools/Shalico/Hierarchy/Group Selection %g")]
        public static void GroupSelection()
        {
            Debug.Log("Group Selection");
            var selection = Selection.gameObjects;
            if (selection.Length == 0)
                return;

            var parent = new GameObject("Group");
            Undo.RegisterCreatedObjectUndo(parent, "Group Selection");
            parent.transform.SetParent(selection[0].transform.parent, false);
            parent.transform.SetSiblingIndex(selection[0].transform.GetSiblingIndex());

            var selectionTransforms = selection
                .Select(x => x.transform)
                .OrderBy(x => x.GetSiblingIndex());

            foreach (var transform in selectionTransforms)
            {
                Undo.SetTransformParent(transform, parent.transform, "Group Selection");
            }
            
            Selection.activeGameObject = parent;
        }
    }

}