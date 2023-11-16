using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShalicoSoundKit
{
    public static class AudioSourceExtensions
    {
        public static async UniTask TweenVolumeUpAsync(this AudioSource audioSource,
            float duration,
            float volume = 1f, CancellationToken cancellationToken = default)
        {
            var initialVolume = audioSource.volume;
            var volumeDiff = volume - initialVolume;

            for (var accumulatedTime = 0f; accumulatedTime < duration; accumulatedTime += Time.deltaTime)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var t = Mathf.Sin(accumulatedTime / duration * Mathf.PI / 2f);
                audioSource.volume = initialVolume + volumeDiff * t;
                await UniTask.Yield(cancellationToken);
            }

            audioSource.volume = volume;
        }

        public static async UniTask TweenVolumeDownAsync(this AudioSource audioSource,
            float duration, float volume = 0, CancellationToken cancellationToken = default)
        {
            var initialVolume = audioSource.volume;
            var volumeDiff = volume - initialVolume;

            for (var accumulatedTime = 0f; accumulatedTime < duration; accumulatedTime += Time.deltaTime)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var t = 1 - Mathf.Cos(accumulatedTime / duration * Mathf.PI / 2f);
                audioSource.volume = initialVolume + volumeDiff * t;
                await UniTask.Yield(cancellationToken);
            }

            audioSource.volume = volume;
        }
    }
}