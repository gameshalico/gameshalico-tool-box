using Cysharp.Threading.Tasks;
using ShalicoAttributePack.Editor;
using ShalicoEffectProcessor.EffectProcessors;
using UnityEditor;
using UnityEngine;

namespace ShalicoEffectProcessor.Editor
{
    [CustomPropertyDrawer(typeof(ChainEffectProcessor))]
    public class ChainEffectProcessorDrawer : PropertyDrawer
    {
        private readonly SubclassListContainerView<ChainEffectProcessor, IEffectProcessor> _view = new("_runners");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EffectProcessorEditorUtility.CloneToggle(property, position);
            if (EffectProcessorEditorUtility.PlayButton(position))
            {
                var value = property.TracePropertyValue();
                if (value is ChainEffectProcessor chainEffectProcessor) chainEffectProcessor.RunAsync().Forget();
            }

            property.isExpanded = EditorGUI.Foldout(
                new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight),
                property.isExpanded, label);
            if (!property.isExpanded)
                return;
            _view.OnGUI(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!property.isExpanded)
                return EditorGUIUtility.singleLineHeight;

            return _view.GetPropertyHeight(property, label);
        }
    }
}