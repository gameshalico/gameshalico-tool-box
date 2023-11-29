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
        [SerializeField] private bool cloneContext;
        [SerializeReference] private IEffectProcessor[] _runners = Array.Empty<IEffectProcessor>();

        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            await RunEffectProcessors(context.CloneIf(cloneContext), (_, _) => UniTask.CompletedTask,
                cancellationToken);
            await function(context, cancellationToken);
        }

        public async UniTask RunFunction(EffectContext context,
            EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            await RunEffectProcessors(context, function, cancellationToken);
        }

        private async UniTask RunEffectProcessors(EffectContext context,
            EffectFunc function,
            CancellationToken cancellationToken, int index = 0)
        {
            if (index < _runners.Length)
                await _runners[index]
                    .Run(context,
                        async (nextContext, nextCancellationToken) =>
                        {
                            await RunEffectProcessors(nextContext, function, nextCancellationToken, index + 1);
                        },
                        cancellationToken);
            else
                await function(context, cancellationToken);
        }
    }
}