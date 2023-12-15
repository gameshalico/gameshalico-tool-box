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
    [CustomDropdownPath("Link/EffectProcessorHolder Link")]
    [CustomListLabel(Tone.Light, HueSymbol.YellowishOrange)]
    public class EffectProcessorHolderLink : UniformEffectProcessor
    {
        [SerializeField] private EffectProcessorHolder effectProcessorHolder;

        protected override async UniTask Run(EffectContext context, CancellationToken cancellationToken)
        {
            await effectProcessorHolder.EffectProcessor.RunAsync(context, cancellationToken);
        }
    }
}