using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [AddEffectProcessorMenu("Multiple/Async Chain")]
    [CustomListLabel("Async Chain", Tone.Light, HueSymbol.RedPurple)]
    public class AsyncChainEffectProcessor : IEffectProcessor
    {
        [SerializeField] private ChainEffectProcessor asyncChain;

        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            asyncChain.Run(context, function, cancellationToken).Forget();
            await function(context, cancellationToken);
        }
    }
}