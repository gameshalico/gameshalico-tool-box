using JetBrains.Annotations;
using UniRx;

namespace ShalicoToolBox
{
    public class TimeSpace
    {
        private readonly HierarchyScaler _hierarchyScaler;
        private readonly PriorityValueArbiter<float> _timeScaleArbiter;

        public TimeSpace(TimeSpace parentTimeSpace = null)
        {
            _hierarchyScaler = new HierarchyScaler();
            _timeScaleArbiter = new PriorityValueArbiter<float>(1f);
            _timeScaleArbiter.ValueReactiveProperty.Subscribe(scale => _hierarchyScaler.SelfScale = scale);
            SetParent(parentTimeSpace);
        }

        public IReadOnlyReactiveProperty<float> TimeScaleReactiveProperty => _hierarchyScaler.ScaleReactiveProperty;

        public static TimeSpace GlobalTimeSpace { get; } = new();

        public float TimeScale => _timeScaleArbiter.ValueReactiveProperty.Value;

        [MustUseReturnValue]
        public IPriorityValueHandler<float> Register(int priority, float timeScale)
        {
            return _timeScaleArbiter.Register(priority, timeScale);
        }

        public void SetDefaultTimeScale(float timeScale)
        {
            _timeScaleArbiter.SetDefaultValue(timeScale);
        }

        public void SetParent(TimeSpace parent)
        {
            if (GlobalTimeSpace == this)
                return;
            _hierarchyScaler.SetParent(parent?._hierarchyScaler ?? GlobalTimeSpace._hierarchyScaler);
        }
    }
}