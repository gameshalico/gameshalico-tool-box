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
    [CustomDropdownPath("Action/Effect")]
    [CustomListLabel(Tone.Light, HueSymbol.Yellow)]
    public class EffectExecutorProcessor : UniformEffectProcessor
    {
        [SerializeField] private bool synchronize = true;
        [SerializeField] private EffectGroup effectGroup;

        protected override async UniTask Run(EffectContext context, CancellationToken cancellationToken)
        {
            var task = effectGroup.PlayAsync(context, cancellationToken);
            if (synchronize)
                await task;
            else
                task.Forget();
        }
    }
}