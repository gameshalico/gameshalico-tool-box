using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShalicoToolBox
{
    public static class EasingFunctions
    {
        public static async IAsyncEnumerable<float> LinearAsyncEnumerable(float duration,
            IDeltaTimeProvider deltaTimeProvider = null,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            deltaTimeProvider ??= DeltaTimeProvider.Scaled;
            float t = 0;

            while (t < duration)
            {
                yield return t / duration;
                
                await UniTask.Yield(playerLoopTiming);
                if (cancellationToken.IsCancellationRequested)
                    yield break;
                t += deltaTimeProvider.ProvideDeltaTime(playerLoopTiming);
            }

            yield return 1;
        }

        public static float Ease(EaseType easeType, float t)
        {
            return easeType switch
            {
                EaseType.Linear => Linear(t),
                EaseType.InQuad => InQuad(t),
                EaseType.OutQuad => OutQuad(t),
                EaseType.InOutQuad => InOutQuad(t),
                EaseType.InCubic => InCubic(t),
                EaseType.OutCubic => OutCubic(t),
                EaseType.InOutCubic => InOutCubic(t),
                EaseType.InQuart => InQuart(t),
                EaseType.OutQuart => OutQuart(t),
                EaseType.InOutQuart => InOutQuart(t),
                EaseType.InQuint => InQuint(t),
                EaseType.OutQuint => OutQuint(t),
                EaseType.InOutQuint => InOutQuint(t),
                EaseType.InSine => InSine(t),
                EaseType.OutSine => OutSine(t),
                EaseType.InOutSine => InOutSine(t),
                EaseType.InExpo => InExpo(t),
                EaseType.OutExpo => OutExpo(t),
                EaseType.InOutExpo => InOutExpo(t),
                EaseType.InCirc => InCirc(t),
                EaseType.OutCirc => OutCirc(t),
                EaseType.InOutCirc => InOutCirc(t),
                EaseType.InBack => InBack(t),
                EaseType.OutBack => OutBack(t),
                EaseType.InOutBack => InOutBack(t),
                EaseType.InElastic => InElastic(t),
                EaseType.OutElastic => OutElastic(t),
                EaseType.InOutElastic => InOutElastic(t),
                EaseType.InBounce => InBounce(t),
                EaseType.OutBounce => OutBounce(t),
                EaseType.InOutBounce => InOutBounce(t),
                _ => throw new ArgumentOutOfRangeException(nameof(easeType), easeType, null)
            };
        }

        public static float Linear(float t)
        {
            return t;
        }

        public static float InQuad(float t)
        {
            return t * t;
        }

        public static float OutQuad(float t)
        {
            return t * (2 - t);
        }

        public static float InOutQuad(float t)
        {
            return t < 0.5f ? 2 * t * t : -1 + (4 - 2 * t) * t;
        }

        public static float InCubic(float t)
        {
            return t * t * t;
        }

        public static float OutCubic(float t)
        {
            return --t * t * t + 1;
        }

        public static float InOutCubic(float t)
        {
            return t < 0.5f ? 4 * t * t * t : (t - 1) * (2 * t - 2) * (2 * t - 2) + 1;
        }

        public static float InQuart(float t)
        {
            return t * t * t * t;
        }

        public static float OutQuart(float t)
        {
            return 1 - --t * t * t * t;
        }

        public static float InOutQuart(float t)
        {
            return t < 0.5f ? 8 * t * t * t * t : 1 - 8 * --t * t * t * t;
        }

        public static float InQuint(float t)
        {
            return t * t * t * t * t;
        }

        public static float OutQuint(float t)
        {
            return 1 + --t * t * t * t * t;
        }

        public static float InOutQuint(float t)
        {
            return t < 0.5f ? 16 * t * t * t * t * t : 1 + 16 * --t * t * t * t * t;
        }

        public static float InSine(float t)
        {
            return 1 - Mathf.Cos(t * Mathf.PI / 2);
        }

        public static float OutSine(float t)
        {
            return Mathf.Sin(t * Mathf.PI / 2);
        }

        public static float InOutSine(float t)
        {
            return -(Mathf.Cos(Mathf.PI * t) - 1) / 2;
        }

        public static float InExpo(float t)
        {
            return Mathf.Pow(2, 10 * (t - 1));
        }

        public static float OutExpo(float t)
        {
            return 1 - Mathf.Pow(2, -10 * t);
        }

        public static float InOutExpo(float t)
        {
            return t < 0.5f ? Mathf.Pow(2, 10 * (2 * t - 1)) / 2 : (2 - Mathf.Pow(2, -10 * (2 * t - 1))) / 2;
        }


        public static float InCirc(float t)
        {
            return 1 - Mathf.Sqrt(1 - t * t);
        }

        public static float OutCirc(float t)
        {
            return Mathf.Sqrt(1 - --t * t);
        }

        public static float InOutCirc(float t)
        {
            return t < 0.5f ? (1 - Mathf.Sqrt(1 - 4 * t * t)) / 2 : (Mathf.Sqrt(1 - 4 * --t * t) + 1) / 2;
        }

        public static float InBack(float t)
        {
            return t * t * (2.70158f * t - 1.70158f);
        }

        public static float OutBack(float t)
        {
            return 1 + --t * t * (2.70158f * t + 1.70158f);
        }

        public static float InOutBack(float t)
        {
            return t < 0.5f
                ? t * t * (7 * t - 2.5f) * 2
                : 1 + --t * t * 2 * (7 * t + 2.5f);
        }

        public static float InElastic(float t)
        {
            return t == 0
                ? 0
                : Mathf.Approximately(t, 1)
                    ? 1
                    : -Mathf.Pow(2, 10 * (t - 1)) * Mathf.Sin((t - 1.1f) * 5 * Mathf.PI);
        }

        public static float OutElastic(float t)
        {
            return t == 0
                ? 0
                : Mathf.Approximately(t, 1)
                    ? 1
                    : Mathf.Pow(2, -10 * t) * Mathf.Sin((t - 0.1f) * 5 * Mathf.PI) + 1;
        }

        public static float InOutElastic(float t)
        {
            return t == 0
                ? 0
                : Mathf.Approximately(t, 1)
                    ? 1
                    : t < 0.5f
                        ? -(Mathf.Pow(2, 20 * t - 10) * Mathf.Sin((20 * t - 11.125f) * Mathf.PI * 2 / 4)) / 2
                        : Mathf.Pow(2, -20 * t + 10) * Mathf.Sin((20 * t - 11.125f) * Mathf.PI * 2 / 4) / 2 + 1;
        }

        public static float InBounce(float t)
        {
            return 1 - OutBounce(1 - t);
        }

        public static float OutBounce(float t)
        {
            if (t < 1.0f / 2.75f)
                return 7.5625f * t * t;
            if (t < 2.0f / 2.75f)
                return 7.5625f * (t -= 1.5f / 2.75f) * t + 0.75f;
            if (t < 2.5f / 2.75f)
                return 7.5625f * (t -= 2.25f / 2.75f) * t + 0.9375f;
            return 7.5625f * (t -= 2.625f / 2.75f) * t + 0.984375f;
        }

        public static float InOutBounce(float t)
        {
            return t < 0.5f ? InBounce(t * 2) / 2 : OutBounce(t * 2 - 1) / 2 + 0.5f;
        }
    }
}