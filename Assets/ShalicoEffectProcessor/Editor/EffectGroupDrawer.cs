using ShalicoEffectProcessor.Effects;
using UnityEditor;
using UnityEngine;

namespace ShalicoEffectProcessor.Editor
{
    [CustomPropertyDrawer(typeof(EffectGroup))]
    public class EffectGroupDrawer : PropertyDrawer
    {
        private readonly SubclassListContainerView<EffectGroup, IEffect> _view =
            new("_effects");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            _view.OnGUI(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _view.GetPropertyHeight(property, label);
        }
    }
}