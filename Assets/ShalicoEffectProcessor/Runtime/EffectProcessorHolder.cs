using UnityEngine;

namespace ShalicoEffectProcessor
{
    public class EffectProcessorHolder : MonoBehaviour
    {
        [SerializeField] private EffectProcessorGroup effectProcessor;

        public EffectProcessorGroup EffectProcessor => effectProcessor;
    }
}