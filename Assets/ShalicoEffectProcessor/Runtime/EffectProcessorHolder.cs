using ShalicoEffectProcessor.EffectProcessors;
using UnityEngine;

namespace ShalicoEffectProcessor
{
    public class EffectProcessorHolder : MonoBehaviour
    {
        [SerializeField] private ChainEffectProcessor effectProcessor;

        public ChainEffectProcessor EffectProcessor => effectProcessor;
    }
}