using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace ShalicoToolBox
{
    public static class TimeScaleArbiter
    {
        private const float DefaultTimeScale = 1f;
        private static readonly PriorityValueArbiter<float> s_timeScaleArbiter = new(DefaultTimeScale);

        static TimeScaleArbiter()
        {
            s_timeScaleArbiter.ValueReactiveProperty.Subscribe(value => { Time.timeScale = value; });
        }

        [MustUseReturnValue]
        public static IPriorityValueHandler<float> Register(int priority, float speed)
        {
            return s_timeScaleArbiter.Register(priority, speed);
        }
    }
}