using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using UnityEngine;

namespace ShalicoFunctionRunner.FunctionRunners
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
            await RunFunctionRunners(0, _ => UniTask.CompletedTask, cancellationToken);
            await function(cancellationToken);
        }

        public async UniTask RunFunction(Func<CancellationToken, UniTask> function,
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