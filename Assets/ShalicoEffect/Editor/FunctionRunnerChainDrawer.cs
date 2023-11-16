using ShalicoEffect.FunctionRunners;
using UnityEditor;
using UnityEngine;

namespace ShalicoEffect.Editor
{
    [CustomPropertyDrawer(typeof(ChainFunctionRunner))]
    public class FunctionRunnerChainDrawer : PropertyDrawer
    {
        private readonly InterfaceListContainerView<ChainFunctionRunner, IFunctionRunner,
            AddFunctionRunnerMenuAttribute> _view = new("_runners");

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