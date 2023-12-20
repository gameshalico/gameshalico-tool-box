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
        public static async IAsyncEnumerable<float> EaseAsyncEnumerable(EasingType easingType, float duration,
            IDeltaTimeProvider deltaTimeProvider = null,
            PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update,
            [EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            if (deltaTimeProvider == null)
                deltaTimeProvider = DeltaTimeProvider.Scaled;

            float t = 0;
            yield return Ease(easingType, 0);

            while (t < duration)
            {
                await UniTask.Yield(playerLoopTiming);
                if (cancellationToken.IsCancellationRequested)
                    yield break;

                yield return Ease(easingType, t / duration);
                t += deltaTimeProvider.ProvideDeltaTime(playerLoopTiming);
            }

            yield return Ease(easingType, 1);
        }

        public static float Ease(EasingType easingType, float t)
        {
            return easingType switch
            {
                EasingType.Linear => Linear(t),
                EasingType.InQuad => InQuad(t),
                EasingType.OutQuad => OutQuad(t),
                EasingType.InOutQuad => InOutQuad(t),
                EasingType.InCubic => InCubic(t),
                EasingType.OutCubic => OutCubic(t),
                EasingType.InOutCubic => InOutCubic(t),
                EasingType.InQuart => InQuart(t),
                EasingType.OutQuart => OutQuart(t),
                EasingType.InOutQuart => InOutQuart(t),
                EasingType.InQuint => InQuint(t),
                EasingType.OutQuint => OutQuint(t),
                EasingType.InOutQuint => InOutQuint(t),
                EasingType.InSine => InSine(t),
                EasingType.OutSine => OutSine(t),
                EasingType.InOutSine => InOutSine(t),
                EasingType.InExpo => InExpo(t),
                EasingType.OutExpo => OutExpo(t),
                EasingType.InOutExpo => InOutExpo(t),
                EasingType.InCirc => InCirc(t),
                EasingType.OutCirc => OutCirc(t),
                EasingType.InOutCirc => InOutCirc(t),
                EasingType.InBack => InBack(t),
                EasingType.OutBack => OutBack(t),
                EasingType.InOutBack => InOutBack(t),
                EasingType.InElastic => InElastic(t),
                EasingType.OutElastic => OutElastic(t),
                EasingType.InOutElastic => InOutElastic(t),
                EasingType.InBounce => InBounce(t),
                EasingType.OutBounce => OutBounce(t),
                EasingType.InOutBounce => InOutBounce(t),
                _ => throw new ArgumentOutOfRangeException(nameof(easingType), easingType, null)
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