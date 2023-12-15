using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;

namespace ShalicoEffectProcessor.EffectProcessors
{
    [Serializable]
    [CustomDropdownPath("Condition/Group Cooldown")]
    [CustomListLabel(Tone.Light, HueSymbol.Green)]
    public class GroupCooldownEffectProcessor : IEffectProcessor
    {
        private static HashSet<string> _groupCooldownSet = new();
        [SerializeField] private TimeScaleMode timeScaleMode;
        [SerializeField] private float cooldown;
        [SerializeField] private string group;

        public bool IsCooldown => _groupCooldownSet.Contains(group);

        public UniTask Run(EffectContext context, EffectFunc function, CancellationToken cancellationToken = default)
        {
            if (IsCooldown) return UniTask.CompletedTask;

            _groupCooldownSet.Add(group);
            Cooldown().Forget();

            return function(context, cancellationToken);
        }

        private async UniTask Cooldown()
        {
            try
            {
                await UniTask.Delay(TimeSpan.FromSeconds(cooldown), timeScaleMode.ToDelayType());
            }
            finally
            {
                _groupCooldownSet.Remove(group);
            }
        }
    }
}