using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffect.Effects;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Effect")]
    [CustomListLabel("Effect", Tone.Light)]
    public class EffectFunctionRunner : IFunctionRunner
    {
        [SerializeField] private EffectGroup effectGroup;

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            await effectGroup.PlayEffectAsync(cancellationToken);
            await function(cancellationToken);
        }
    }
}