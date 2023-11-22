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

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            asyncChain.Run(function, cancellationToken).Forget();
            await function(cancellationToken);
        }
    }
}