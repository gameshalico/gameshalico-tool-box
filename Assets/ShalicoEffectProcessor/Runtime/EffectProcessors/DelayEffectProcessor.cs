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
    [CustomDropdownPath("Delay/Delay")]
    [CustomListLabel("Delay", Tone.Light, HueSymbol.Blue2)]
    public class DelayEffectProcessor : IEffectProcessor
    {
        [SerializeField] private TimeScaleMode timeScaleMode;
        [SerializeField] private float delay;

        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), timeScaleMode.ToDelayType(),
                cancellationToken: cancellationToken);
            await function(context, cancellationToken);
        }
    }
}