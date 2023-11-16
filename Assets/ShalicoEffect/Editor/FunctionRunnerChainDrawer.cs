using Cysharp.Threading.Tasks;
using ShalicoAttributePack.Editor;
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
            // play button
            var runButtonRect = new Rect(position.x + position.width - EditorGUIUtility.singleLineHeight - 55,
                position.y, 50,
                EditorGUIUtility.singleLineHeight);
            if (GUI.Button(runButtonRect, "Run"))
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