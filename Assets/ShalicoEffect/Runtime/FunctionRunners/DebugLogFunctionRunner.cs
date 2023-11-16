using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.FunctionRunners
{
    [Serializable]
    [CustomListLabel("Debug Log", Tone.Light, HueSymbol.Yellow)]
    [AddFunctionRunnerMenu("Action/Debug Log", -1)]
    public class DebugLogFunctionRunner : IFunctionRunner
    {
        [SerializeField] private string message;

        public UniTask Run(Func<CancellationToken, UniTask> function, CancellationToken cancellationToken = default)
        {
            Debug.Log(message);
            return function(cancellationToken);
        }
    }
}