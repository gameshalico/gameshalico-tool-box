using JetBrains.Annotations;
using UniRx;
using UnityEngine;

namespace ShalicoToolBox
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorSpeedArbiter : MonoBehaviour
    {
        [SerializeField] private float defaultSpeed = 1f;
        private Animator _animator;
        private PriorityValueArbiter<float> _speedArbiter;

        private void Awake()
        {
            _animator = GetComponent<Animator>();

            _speedArbiter = new PriorityValueArbiter<float>(defaultSpeed);
            _speedArbiter.ValueReactiveProperty.Subscribe(speed => _animator.speed = speed);
        }

        [MustUseReturnValue]
        public IPriorityValueHandler<float> Register(int priority, float speed)
        {
            return _speedArbiter.Register(priority, speed);
        }
    }
}