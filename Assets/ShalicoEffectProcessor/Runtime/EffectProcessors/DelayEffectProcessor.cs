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
    [CustomListLabel(Tone.Light, HueSymbol.Blue2)]
    public class DelayEffectProcessor : UniformEffectProcessor
    {
        [SerializeField] private TimeScaleMode timeScaleMode;
        [SerializeField] private float delay;

        protected override async UniTask Run(EffectContext context, CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), timeScaleMode.ToDelayType(),
                cancellationToken: cancellationToken);
        }
    }
}