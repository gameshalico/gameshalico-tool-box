using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [AddEffectProcessorMenu("Condition/Active")]
    [CustomListLabel("Active", Tone.Light, HueSymbol.Green)]
    public class ActiveEffectProcessor : IEffectProcessor
    {
        [SerializeField] private bool active = true;

        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            if (active)
                await function(context, cancellationToken);
        }
    }
}