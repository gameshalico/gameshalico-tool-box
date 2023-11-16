using UnityEditor;
using UnityEngine;

namespace ShalicoEffect.Editor
{
    public static class FunctionRunnerEditorUtility
    {
        public static bool PlayButton(Rect rect)
        {
            var runButtonRect = new Rect(rect.x + rect.width - EditorGUIUtility.singleLineHeight - 55,
                rect.y, 50,
                EditorGUIUtility.singleLineHeight);
            return GUI.Button(runButtonRect, "Run");
        }
    }
}