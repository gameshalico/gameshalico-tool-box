using UnityEngine;

namespace ShalicoToolBox.TimeControllers
{
    public class TimeScaleController : ITimeSpaceController
    {
        public void OnTimeScaleChanged(float scale)
        {
            Time.timeScale = scale;
        }
    }
}