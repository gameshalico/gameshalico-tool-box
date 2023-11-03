using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShalicoEffect
{
    public enum TimeScaleMode
    {
        Scaled,
        Unscaled
    }

    [Serializable]
    public struct EffectTiming
    {
        [Header("TimeScale")] public TimeScaleMode timeScaleMode;
        [Header("Delay")] public float initialDelay;

        public float cooldownDuration;

        [Header("Repeat")] public int repeatCount;

        public bool repeatForever;
        public float repeatDelay;

        public EffectTiming(
            float initialDelay,
            float cooldownDuration,
            int repeatCount,
            bool repeatForever,
            float repeatDelay,
            TimeScaleMode timeScaleMode)
        {
            this.initialDelay = initialDelay;
            this.cooldownDuration = cooldownDuration;
            this.repeatCount = repeatCount;
            this.repeatForever = repeatForever;
            this.repeatDelay = repeatDelay;
            this.timeScaleMode = timeScaleMode;
        }
    }

    [Serializable]
    public abstract class Effect : IEffect
    {
        [SerializeField] private bool isEnabled = true;
        [SerializeField] private EffectTiming timing;
        private bool _isCooldown;

        public bool IsEnabled
        {
            get => isEnabled;
            set => isEnabled = value;
        }

        protected bool IgnoreTimeScale => timing.timeScaleMode == TimeScaleMode.Unscaled;

        async UniTask IEffect.PlayAsync(CancellationToken cancellationToken)
        {
            if (!IsEnabled)
                return;

            if (timing.cooldownDuration > 0)
            {
                if (_isCooldown)
                    return;

                CooldownAsync(cancellationToken).Forget();
            }

            await UniTask.Delay(
                TimeSpan.FromSeconds(timing.initialDelay),
                IgnoreTimeScale, cancellationToken: cancellationToken);

            var repeat = timing.repeatCount + 1;

            do
            {
                await PlayEffectAsync(cancellationToken);
                await UniTask.Delay(
                    TimeSpan.FromSeconds(timing.repeatDelay),
                    IgnoreTimeScale, cancellationToken: cancellationToken);
                repeat--;
            } while (timing.repeatForever || repeat > 0);
        }

        private async UniTask CooldownAsync(CancellationToken cancellationToken)
        {
            _isCooldown = true;
            await UniTask.Delay(
                TimeSpan.FromSeconds(timing.cooldownDuration),
                IgnoreTimeScale, cancellationToken: cancellationToken);
            _isCooldown = false;
        }

        protected abstract UniTask PlayEffectAsync(CancellationToken cancellationToken);
    }
}