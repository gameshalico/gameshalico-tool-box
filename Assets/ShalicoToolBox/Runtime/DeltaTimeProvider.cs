using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShalicoToolBox
{
    public class DeltaTimeProvider
    {
        public static IDeltaTimeProvider Scaled { get; } = new ScaledDeltaTimeProvider();
        public static IDeltaTimeProvider Unscaled { get; } = new UnscaledDeltaTimeProvider();

        public static float GetScaledDeltaTime(PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update)
        {
            return playerLoopTiming switch
            {
                PlayerLoopTiming.Update => Time.deltaTime,
                PlayerLoopTiming.FixedUpdate => Time.fixedDeltaTime,
                _ => Time.deltaTime
            };
        }

        public static float GetUnscaledDeltaTime(PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update)
        {
            return playerLoopTiming switch
            {
                PlayerLoopTiming.Update => Time.unscaledDeltaTime,
                PlayerLoopTiming.FixedUpdate => Time.fixedUnscaledDeltaTime,
                _ => Time.unscaledDeltaTime
            };
        }

        public class ScaledDeltaTimeProvider : IDeltaTimeProvider
        {
            public float ProvideDeltaTime(PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update)
            {
                return GetScaledDeltaTime(playerLoopTiming);
            }
        }

        public class UnscaledDeltaTimeProvider : IDeltaTimeProvider
        {
            public float ProvideDeltaTime(PlayerLoopTiming playerLoopTiming = PlayerLoopTiming.Update)
            {
                return GetUnscaledDeltaTime(playerLoopTiming);
            }
        }
    }
}