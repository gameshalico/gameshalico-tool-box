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
    [CustomDropdownPath("Multiple/Async Chain")]
    [CustomListLabel(Tone.Light, HueSymbol.RedPurple)]
    public class AsyncChainEffectProcessor : IEffectProcessor
    {
        [SerializeField] private ChainEffectProcessor asyncChain;

        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            asyncChain.RunAsync(context.AddRef(), cancellationToken).Forget();
            await function(context, cancellationToken);
        }
    }
}