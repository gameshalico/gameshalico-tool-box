using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffectProcessor.Context;
using ShalicoEffectProcessor.EffectProcessors;
using UnityEngine;

namespace ShalicoEffectProcessor
{
    [Serializable]
    public class EffectPlayer : IDisposable
    {
        [SerializeField] private ChainEffectProcessor effectProcessor;

        public EffectPlayer()
        {
            Context = EffectContext.Rent();
            Context.AddRef();
        }

        public EffectContext Context { get; private set; }

        public void Dispose()
        {
            Context.Release();
            Context = null;
        }

        public void Play(CancellationToken cancellationToken = default)
        {
            effectProcessor.Run(Context, cancellationToken);
        }

        public async UniTask PlayAsync(CancellationToken cancellationToken = default)
        {
            await effectProcessor.RunAsync(Context, cancellationToken);
        }
    }
}