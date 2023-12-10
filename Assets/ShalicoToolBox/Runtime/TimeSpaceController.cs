using UniRx;
using UnityEngine;

namespace ShalicoToolBox
{
    public abstract class TimeSpaceController : MonoBehaviour
    {
        [SerializeField] private TimeSpaceContainer timeSpaceContainer;

        private void Awake()
        {
            if (timeSpaceContainer == null)
                TimeSpace.GlobalTimeSpace.TimeScaleReactiveProperty.Subscribe(OnTimeScaleChanged);

            timeSpaceContainer.TimeSpace.TimeScaleReactiveProperty.Subscribe(OnTimeScaleChanged);
        }

        protected abstract void OnTimeScaleChanged(float scale);
    }
}