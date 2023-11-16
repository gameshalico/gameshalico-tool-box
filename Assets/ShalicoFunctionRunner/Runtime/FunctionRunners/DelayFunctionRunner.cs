using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoFunctionRunner.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Delay")]
    [CustomListLabel("Delay", Tone.Light, HueSymbol.Blue2)]
    public class DelayFunctionRunner : IFunctionRunner
    {
        [SerializeField] private TimeScaleMode timeScaleMode;
        [SerializeField] private float delay;

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(delay), timeScaleMode.ToDelayType(),
                cancellationToken: cancellationToken);
            await function(cancellationToken);
        }
    }
}