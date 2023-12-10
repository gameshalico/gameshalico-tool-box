using UnityEditor;
using UnityEngine;

namespace ShalicoEffectProcessor.Editor
{
    public static class EffectProcessorEditorUtility
    {
        public static bool PlayButton(Rect rect)
        {
            var runButtonRect = new Rect(rect.x + rect.width - EditorGUIUtility.singleLineHeight - 55,
                rect.y, 50,
                EditorGUIUtility.singleLineHeight);

            return GUI.Button(runButtonRect, "Run");
        }

        public static void CloneToggle(SerializedProperty property, Rect rect)
        {
            var cloneContextProperty = property.FindPropertyRelative("cloneContext");

            var cloneContextLabelRect = new Rect(rect.x + rect.width - 120,
                rect.y, 20,
                EditorGUIUtility.singleLineHeight);
            var cloneContextRect = new Rect(rect.x + rect.width - 100,
                rect.y, 15,
                EditorGUIUtility.singleLineHeight);

            var iconTexture = EditorGUIUtility.Load("d_Preset.Context") as Texture2D;
            var content = new GUIContent(iconTexture, "Clone EffectContext on each run");

            GUI.Label(cloneContextLabelRect, content);
            EditorGUI.PropertyField(cloneContextRect, cloneContextProperty, GUIContent.none);
        }
    }
}