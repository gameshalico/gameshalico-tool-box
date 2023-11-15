using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.FunctionRunners
{
    [Serializable]
    [CustomListLabel("Parallel", Tone.Light, HueSymbol.RedPurple)]
    [AddFunctionRunnerMenu("Parallel")]
    public class ParallelFunctionRunner : IFunctionRunner
    {
        [SerializeField] private FunctionRunnerChain[] chains = Array.Empty<FunctionRunnerChain>();

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            await UniTask.WhenAll(chains.Select(chain => chain.Run(function, cancellationToken)));
        }
    }
}