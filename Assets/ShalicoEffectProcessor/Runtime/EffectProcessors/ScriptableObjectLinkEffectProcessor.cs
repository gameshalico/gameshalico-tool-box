using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [CustomDropdownPath("Link/EffectProcessorScriptableObject Link")]
    [CustomListLabel(Tone.Light, HueSymbol.YellowishOrange)]
    public class ScriptableObjectLinkEffectProcessor : UniformEffectProcessor
    {
        [SerializeField] private EffectProcessorScriptableObject scriptableObject;

        protected override async UniTask Run(EffectContext context, CancellationToken cancellationToken)
        {
            await scriptableObject.EffectProcessor.RunAsync(context, cancellationToken);
        }
    }
}