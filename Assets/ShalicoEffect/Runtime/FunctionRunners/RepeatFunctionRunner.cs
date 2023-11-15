using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoAttributePack;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Repeat")]
    [CustomListLabel("Repeat", Tone.Light)]
    public class RepeatFunctionRunner : IFunctionRunner
    {
        [EnableIf(nameof(repeatForever), false)] [SerializeField]
        private int repeatCount;

        [SerializeField] private bool repeatForever;

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            var repeat = repeatCount + 1;

            do
            {
                await function(cancellationToken);
                repeat--;
            } while (repeatForever || repeat > 0);
        }
    }
}