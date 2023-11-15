using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShalicoEffect.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Delay")]
    public class DelayFunctionRunner : IFunctionRunner
    {
        [SerializeField] private float delay;

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), cancellationToken: cancellationToken);
            function(cancellationToken);
        }
    }
}