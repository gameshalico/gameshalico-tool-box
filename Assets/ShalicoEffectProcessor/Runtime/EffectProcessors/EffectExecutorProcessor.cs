using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using ShalicoEffectProcessor.Effects;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [AddEffectProcessorMenu("Action/Effect", 1)]
    [CustomListLabel("Effect", Tone.Light, HueSymbol.Yellow)]
    public class EffectExecutorProcessor : IEffectProcessor
    {
        [SerializeField] private bool synchronize = true;
        [SerializeField] private EffectGroup effectGroup;

        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            var task = effectGroup.PlayEffectAsync(context, cancellationToken);
            if (synchronize)
                await task;
            else
                task.Forget();

            await function(context, cancellationToken);
        }
    }
}