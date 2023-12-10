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
    [CustomDropdownPath("Multiple/Chain")]
    [CustomListLabel("Chain", Tone.Light, HueSymbol.RedPurple)]
    public class ChainEffectProcessor : IEffectProcessor
    {
        [SerializeField] private bool cloneContext;
        [SerializeReference] private IEffectProcessor[] _runners = Array.Empty<IEffectProcessor>();

        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            var subContext = cloneContext ? context.Clone() : context.AddRef();
            try
            {
                await RunEffectProcessors(subContext, (_, _) => UniTask.CompletedTask, cancellationToken);
            }
            finally
            {
                subContext.Release();
            }

            await function(context, cancellationToken);
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