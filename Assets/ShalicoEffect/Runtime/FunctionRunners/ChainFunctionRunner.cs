using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Multiple/Chain", 1)]
    [CustomListLabel("Chain", Tone.Light, HueSymbol.RedPurple)]
    public class ChainFunctionRunner : IFunctionRunner
    {
        [SerializeReference] private IFunctionRunner[] _runners = Array.Empty<IFunctionRunner>();

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            await RunFunctionRunners(0, function, cancellationToken);
        }

        private async UniTask RunFunctionRunners(int index, Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken)
        {
            if (index < _runners.Length)
                await _runners[index]
                    .Run(async token => { await RunFunctionRunners(index + 1, function, token); }, cancellationToken);
            else
                await function(cancellationToken);
        }
    }
}