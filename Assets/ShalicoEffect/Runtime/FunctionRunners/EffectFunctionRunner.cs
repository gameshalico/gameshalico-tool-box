using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffect.Effects;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Action/Effect", 1)]
    [CustomListLabel("Effect", Tone.Light, HueSymbol.Yellow)]
    public class EffectFunctionRunner : IFunctionRunner
    {
        [SerializeField] private bool synchronize = true;
        [SerializeField] private EffectGroup effectGroup;

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            var task = effectGroup.PlayEffectAsync(cancellationToken);
            if (synchronize)
                await task;
            else
                task.Forget();

            await function(cancellationToken);
        }
    }
}