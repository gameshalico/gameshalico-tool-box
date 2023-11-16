using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoFunctionRunner.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Condition/Active")]
    [CustomListLabel("Active", Tone.Light, HueSymbol.Green)]
    public class ActiveFunctionRunner : IFunctionRunner
    {
        [SerializeField] private bool active = true;

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            if (active)
                await function(cancellationToken);
        }
    }
}