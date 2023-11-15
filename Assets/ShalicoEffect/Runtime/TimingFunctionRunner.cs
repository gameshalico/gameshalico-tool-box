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

    /// <summary>
    ///     ディレイやリピート、クールダウンなど、タイミングを制御して関数を実行するクラス。
    /// </summary>
    [Serializable]
    public class TimingFunctionRunner : IFunctionRunner
    {
        [SerializeField] [Header("Delay")] private float initialDelay;

        [SerializeField] private float cooldownDuration;

        [SerializeField] [Header("Repeat")] private int repeatCount;

        [SerializeField] private bool repeatForever;
        [SerializeField] private float repeatDelay;
        [SerializeField] [Header("TimeScale")] private TimeScaleMode timeScaleMode;
        private bool _isCooldown;
        protected bool IgnoreTimeScale => timeScaleMode == TimeScaleMode.Unscaled;

        public async UniTask Run(Func<CancellationToken, UniTask> function,
            CancellationToken cancellationToken = default)
        {
            if (cooldownDuration > 0)
            {
                if (_isCooldown)
                    return;

                CooldownAsync(cancellationToken).Forget();
            }

            await UniTask.Delay(
                TimeSpan.FromSeconds(initialDelay),
                IgnoreTimeScale, cancellationToken: cancellationToken);

            var repeat = repeatCount + 1;

            do
            {
                await function(cancellationToken);
                await UniTask.Delay(
                    TimeSpan.FromSeconds(repeatDelay),
                    IgnoreTimeScale, cancellationToken: cancellationToken);
                repeat--;
            } while (repeatForever || repeat > 0);
        }

        private async UniTask CooldownAsync(CancellationToken cancellationToken)
        {
            _isCooldown = true;
            await UniTask.Delay(
                TimeSpan.FromSeconds(cooldownDuration),
                IgnoreTimeScale, cancellationToken: cancellationToken);
            _isCooldown = false;
        }
    }
}