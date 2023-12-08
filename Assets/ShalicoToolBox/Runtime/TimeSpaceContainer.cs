using UnityEngine;

namespace ShalicoToolBox
{
    public class TimeSpaceContainer : MonoBehaviour
    {
        [SerializeField] private float defaultTimeScale = 1f;
        [SerializeField] private TimeSpaceContainer parentContainer;
        public TimeSpace TimeSpace { get; } = new();

        private void Awake()
        {
            TimeSpace.SetDefaultTimeScale(defaultTimeScale);
            if (parentContainer != null)
                TimeSpace.SetParent(parentContainer.TimeSpace);
        }
    }
}