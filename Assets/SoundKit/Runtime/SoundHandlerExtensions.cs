using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace SoundKit
{
    public static class SoundHandlerExtensions
    {
        public static ISoundHandler PlayWithFadeIn(this ISoundHandler handler, float duration, float volume = 1f)
        {
            handler.Play(0f);
            handler.TweenVolumeUpAsync(duration, volume).Forget();
            return handler;
        }

        public static async UniTask<ISoundHandler> PlayWithFadeInAsync(this ISoundHandler handler, float duration,
            float volume = 1f,
            CancellationToken cancellationToken = default)
        {
            handler.Play(0f);
            await handler.TweenVolumeUpAsync(duration, volume, cancellationToken);
            return handler;
        }

        public static ISoundHandler FadeOutAndStop(this ISoundHandler handler, float duration)
        {
            handler.FadeOutAndStopAsync(duration).Forget();
            return handler;
        }

        public static async UniTask<ISoundHandler> FadeOutAndStopAsync(this ISoundHandler handler, float duration,
            CancellationToken cancellationToken = default)
        {
            await handler.TweenVolumeDownAsync(duration, 0f, cancellationToken);
            return handler.Stop();
        }

        public static ISoundHandler SetPitchByEqualTemperament(this ISoundHandler handler, int pitch)
        {
            return handler.SetPitch(Mathf.Pow(2f, pitch / 12f));
        }

        public static ISoundHandler SetPitchByJustIntonation(this ISoundHandler handler, int pitch)
        {
            return handler.SetPitch(Mathf.Pow(2f, pitch / 7f));
        }

        public static ISoundHandler SetRandomPitch(this ISoundHandler handler, float min, float max)
        {
            return handler.SetPitch(Random.Range(min, max));
        }

        public static ISoundHandler SetRandomPitchByEqualTemperament(this ISoundHandler handler, int min, int max)
        {
            return handler.SetPitchByEqualTemperament(Random.Range(min, max));
        }

        public static ISoundHandler SetRandomPitchByJustIntonation(this ISoundHandler handler, int min, int max)
        {
            return handler.SetPitchByJustIntonation(Random.Range(min, max));
        }

        public static void CrossFade(this ISoundHandler handler, ISoundHandler other, float duration)
        {
            handler.FadeOutAndStopAsync(duration).Forget();
            other.PlayWithFadeIn(duration);
        }

        public static async UniTask CrossFadeAsync(this ISoundHandler handler, ISoundHandler other, float duration,
            CancellationToken cancellationToken = default)
        {
            await UniTask.WhenAll(handler.FadeOutAndStopAsync(duration, cancellationToken),
                other.PlayWithFadeInAsync(duration, cancellationToken: cancellationToken));
        }
    }
}