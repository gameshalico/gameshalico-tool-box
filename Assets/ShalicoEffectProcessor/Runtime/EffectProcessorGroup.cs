using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffectProcessor.Context;
using ShalicoEffectProcessor.EffectProcessors;
using UnityEngine;

namespace ShalicoEffectProcessor
{
    [Serializable]
    public class EffectProcessorGroup
    {
        [SerializeField] private ChainEffectProcessor effectProcessor;

        public void Run(CancellationToken cancellationToken = default)
        {
            effectProcessor.Run(cancellationToken);
        }

        public void Run(EffectContext context, CancellationToken cancellationToken = default)
        {
            effectProcessor.Run(context, cancellationToken);
        }

        public async UniTask RunAsync(CancellationToken cancellationToken = default)
        {
            await effectProcessor.RunAsync(cancellationToken);
        }

        public async UniTask RunAsync(EffectContext context, CancellationToken cancellationToken = default)
        {
            await effectProcessor.RunAsync(context, cancellationToken);
        }
    }
}