using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SoundKit.Obsolute
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
                if (cancellationToken.IsCancellationRequested) return;

                var t = (Mathf.Sin(accumulatedTime / duration * 2f) + 1f) / 2f;
                audioSource.volume = initialVolume + volumeDiff * t;
                await UniTask.Yield();
            }
        }

        public static async UniTask TweenVolumeDownAsync(this AudioSource audioSource,
            float duration, float volume = 0, CancellationToken cancellationToken = default)
        {
            if (!audioSource.isPlaying) return;

            var initialVolume = audioSource.volume;
            var volumeDiff = initialVolume - volume;

            for (var accumulatedTime = 0f; accumulatedTime < duration; accumulatedTime += Time.deltaTime)
            {
                if (cancellationToken.IsCancellationRequested) return;

                var t = (Mathf.Cos(accumulatedTime / duration * 2f) + 1f) / 2f;
                audioSource.volume = initialVolume - volumeDiff * t;
                await UniTask.Yield();
            }
        }
    }
}