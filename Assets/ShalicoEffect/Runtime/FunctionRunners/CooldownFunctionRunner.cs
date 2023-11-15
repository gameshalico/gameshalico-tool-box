using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.FunctionRunners
{
    [Serializable]
    [AddFunctionRunnerMenu("Cooldown")]
    [CustomListLabel("Cooldown", Tone.Light, HueSymbol.Blue2)]
    public class CooldownFunctionRunner : IFunctionRunner
    {
        [SerializeField] private TimeScaleMode timeScaleMode;
        [SerializeField] private float cooldown;
        private bool _isCooldown;

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            if (_isCooldown) return;

            _isCooldown = true;
            Cooldown(cancellationToken).Forget();

            await function(cancellationToken);
        }

        private async UniTask Cooldown(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(cooldown), timeScaleMode.ToDelayType(),
                cancellationToken: cancellationToken);
            _isCooldown = false;
        }
    }
}