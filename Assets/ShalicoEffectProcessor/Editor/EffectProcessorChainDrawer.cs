using Cysharp.Threading.Tasks;
using ShalicoAttributePack.Editor;
using ShalicoEffectProcessor.EffectProcessors;
using UnityEditor;
using UnityEngine;

namespace ShalicoEffectProcessor.Editor
{
    [CustomPropertyDrawer(typeof(ChainEffectProcessor))]
    public class EffectProcessorChainDrawer : PropertyDrawer
    {
        private readonly InterfaceListContainerView<ChainEffectProcessor, IEffectProcessor,
            AddEffectProcessorMenuAttribute> _view = new("_runners");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (EffectProcessorEditorUtility.PlayButton(position))
            {
                var value = property.TracePropertyValue();
                if (value is ChainEffectProcessor chainEffectProcessor) chainEffectProcessor.RunAsync().Forget();
            }

            _view.OnGUI(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _view.GetPropertyHeight(property, label);
        }
    }
}