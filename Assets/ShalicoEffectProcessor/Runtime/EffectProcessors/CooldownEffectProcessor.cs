using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [CustomDropdownPath("Condition/Cooldown")]
    [CustomListLabel(Tone.Light, HueSymbol.Green)]
    public class CooldownEffectProcessor : IEffectProcessor
    {
        [SerializeField] private TimeScaleMode timeScaleMode;
        [SerializeField] private float cooldown;
        private bool _isCooldown;

        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            if (_isCooldown) return;

            _isCooldown = true;
            Cooldown(cancellationToken).Forget();

            await function(context, cancellationToken);
        }

        private async UniTask Cooldown(CancellationToken cancellationToken)
        {
            await UniTask.Delay(TimeSpan.FromSeconds(cooldown), timeScaleMode.ToDelayType(),
                cancellationToken: cancellationToken);
            _isCooldown = false;
        }
    }
}