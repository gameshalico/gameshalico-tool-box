using Cysharp.Threading.Tasks;

namespace ShalicoEffectProcessor
{
    public enum TimeScaleMode
    {
        Scaled,
        Unscaled,
        Realtime
    }

    public static class TimeScaleModeExtensions
    {
        public static DelayType ToDelayType(this TimeScaleMode timeScaleMode)
        {
            return timeScaleMode switch
            {
                TimeScaleMode.Scaled => DelayType.DeltaTime,
                TimeScaleMode.Unscaled => DelayType.UnscaledDeltaTime,
                TimeScaleMode.Realtime => DelayType.Realtime,
                _ => DelayType.DeltaTime
            };
        }
    }
}