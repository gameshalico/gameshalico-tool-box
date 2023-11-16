using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoFunctionRunner.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Multiple/Async Chain")]
    [CustomListLabel("Async Chain", Tone.Light, HueSymbol.RedPurple)]
    public class AsyncChainFunctionRunner : IFunctionRunner
    {
        [SerializeField] private ChainFunctionRunner asyncChain;

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            asyncChain.Run(function, cancellationToken).Forget();
            await function(cancellationToken);
        }
    }
}