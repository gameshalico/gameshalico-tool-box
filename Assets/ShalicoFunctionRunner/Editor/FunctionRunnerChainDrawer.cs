using Cysharp.Threading.Tasks;
using ShalicoAttributePack.Editor;
using ShalicoFunctionRunner.FunctionRunners;
using UnityEditor;
using UnityEngine;

namespace ShalicoFunctionRunner.Editor
{
    [CustomPropertyDrawer(typeof(ChainFunctionRunner))]
    public class FunctionRunnerChainDrawer : PropertyDrawer
    {
        private readonly InterfaceListContainerView<ChainFunctionRunner, IFunctionRunner,
            AddFunctionRunnerMenuAttribute> _view = new("_runners");

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (FunctionRunnerEditorUtility.PlayButton(position))
            {
                var value = property.TracePropertyValue();
                if (value is ChainFunctionRunner chainFunctionRunner) chainFunctionRunner.RunAsync().Forget();
            }

            _view.OnGUI(position, property, label);
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return _view.GetPropertyHeight(property, label);
        }
    }
}