using System;
using UnityEngine;

namespace ShalicoToolBox.TimeControllers
{
    [Serializable]
    public class AnimatorSpeedController : ITimeSpaceController
    {
        [SerializeField] private Animator animator;

        public void OnTimeScaleChanged(float scale)
        {
            animator.speed = scale;
        }
    }
}