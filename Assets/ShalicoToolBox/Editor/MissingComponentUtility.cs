using UnityEditor;
using UnityEngine;

namespace Shalico.ToolBox.Editor
{
    public static class MissingComponentUtility
    {
        [MenuItem("Tools/Shalico/Missing Scripts/Count Missing Scripts")]
        private static void LogMissingScriptCount()
        {
            GameObject gameObject = Selection.activeGameObject;
            if (gameObject == null)
            {
                return;
            }

            int missingCount = GameObjectUtility.GetMonoBehavioursWithMissingScriptCount(gameObject);
            Debug.Log(missingCount);
        }

        [MenuItem("Tools/Shalico/Missing Scripts/Remove Missing Scripts")]
        private static void RemoveMissingScripts()
        {
            GameObject gameObject = Selection.activeGameObject;
            if (gameObject == null)
            {
                return;
            }

            int missingCount = GameObjectUtility.RemoveMonoBehavioursWithMissingScript(gameObject);
            Debug.Log(missingCount);
        }
    }
}