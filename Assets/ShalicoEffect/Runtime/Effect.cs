using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffect.FunctionRunners;
using UnityEngine;

namespace ShalicoEffect
{
    [Serializable]
    public abstract class Effect : IEffect
    {
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private FunctionRunnerChain runnerChain;

        async UniTask IEffect.PlayEffectAsync(CancellationToken cancellationToken)
        {
            if (!isEnabled)
                return;
            await runnerChain.Run(async token => { await PlayEffectWithTimingAsync(token); }, cancellationToken);
        }

        protected abstract UniTask PlayEffectWithTimingAsync(CancellationToken cancellationToken);
    }
}