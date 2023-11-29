using System;
using Cysharp.Threading.Tasks;
using ShalicoAttributePack.Editor;
using ShalicoEffectProcessor.EffectProcessors;
using UnityEditor;
using UnityEngine;

namespace ShalicoEffectProcessor.Editor
{
    [Serializable]
    [CustomPropertyDrawer(typeof(ParallelChainEffectProcessor))]
    public class ParallelChainDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (EffectProcessorEditorUtility.PlayButton(position))
            {
                var value = property.TracePropertyValue();
                if (value is ParallelChainEffectProcessor chainEffectProcessor)
                    chainEffectProcessor.RunAsync().Forget();
            }

            EditorGUI.PropertyField(position, property, label, true);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label);
        }
    }
}