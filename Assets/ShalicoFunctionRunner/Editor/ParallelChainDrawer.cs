using System;
using Cysharp.Threading.Tasks;
using ShalicoAttributePack.Editor;
using ShalicoFunctionRunner.FunctionRunners;
using UnityEditor;
using UnityEngine;

namespace ShalicoFunctionRunner.Editor
{
    [Serializable]
    [CustomPropertyDrawer(typeof(ParallelChainFunctionRunner))]
    public class ParallelChainDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (FunctionRunnerEditorUtility.PlayButton(position))
            {
                var value = property.TracePropertyValue();
                if (value is ParallelChainFunctionRunner chainFunctionRunner) chainFunctionRunner.RunAsync().Forget();
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}