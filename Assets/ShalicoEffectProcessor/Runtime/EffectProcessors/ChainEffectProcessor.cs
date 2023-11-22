using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [AddEffectProcessorMenu("Multiple/Chain", 1)]
    [CustomListLabel("Chain", Tone.Light, HueSymbol.RedPurple)]
    public class ChainEffectProcessor : IEffectProcessor
    {
        [SerializeReference] private IEffectProcessor[] _runners = Array.Empty<IEffectProcessor>();

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            await RunEffectProcessors(0, _ => UniTask.CompletedTask, cancellationToken);
            await function(cancellationToken);
        }

        public async UniTask RunFunction(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            await RunEffectProcessors(0, function, cancellationToken);
        }

        private async UniTask RunEffectProcessors(int index, Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken)
        {
            if (index < _runners.Length)
                await _runners[index]
                    .Run(async token => { await RunEffectProcessors(index + 1, function, token); }, cancellationToken);
            else
                await function(cancellationToken);
        }
    }
}