using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using UnityEngine;

namespace ShalicoFunctionRunner.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Multiple/Parallel Chain")]
    [CustomListLabel("Parallel Chain", Tone.Light, HueSymbol.RedPurple)]
    public class ParallelChainFunctionRunner : IFunctionRunner
    {
        [SerializeField] private ChainFunctionRunner[] chains = Array.Empty<ChainFunctionRunner>();

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            await UniTask.WhenAll(chains.Select(chain => chain.Run(function, cancellationToken)));

            await function(cancellationToken);
        }
    }
}