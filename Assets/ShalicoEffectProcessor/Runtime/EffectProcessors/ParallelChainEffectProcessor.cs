using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [AddEffectProcessorMenu("Multiple/Parallel Chain")]
    [CustomListLabel("Parallel Chain", Tone.Light, HueSymbol.RedPurple)]
    public class ParallelChainEffectProcessor : IEffectProcessor
    {
        [SerializeField] private ChainEffectProcessor[] chains = Array.Empty<ChainEffectProcessor>();

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            await UniTask.WhenAll(chains.Select(chain => chain.Run(function, cancellationToken)));

            await function(cancellationToken);
        }
    }
}