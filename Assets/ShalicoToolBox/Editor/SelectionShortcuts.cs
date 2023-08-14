using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Shalico.ToolBox.Editor
{
    public static class SelectionShortcuts
    {
        [MenuItem("Tools/Shalico/Group Selection %g")]
        public static void GroupSelection()
        {
            Debug.Log("Group Selection");
            GameObject[] selection = Selection.gameObjects;
            if (selection.Length == 0)
            {
                return;
            }

            GameObject parent = new("Group");
            Undo.RegisterCreatedObjectUndo(parent, "Group Selection");
            parent.transform.SetParent(selection[0].transform.parent, false);
            parent.transform.SetSiblingIndex(selection[0].transform.GetSiblingIndex());

            IOrderedEnumerable<Transform> selectionTransforms = selection
                .Select(x => x.transform)
                .OrderBy(x => x.GetSiblingIndex());

            foreach (Transform transform in selectionTransforms)
            {
                Undo.SetTransformParent(transform, parent.transform, "Group Selection");
            }

            Selection.activeGameObject = parent;
        }
    }
}