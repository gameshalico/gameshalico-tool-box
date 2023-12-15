using ShalicoEffectProcessor.EffectProcessors;
using UnityEngine;

namespace ShalicoEffectProcessor
{
    [CreateAssetMenu(fileName = "EffectProcessorScriptableObject", menuName = "Shalico/EffectProcessor", order = 0)]
    public class EffectProcessorScriptableObject : ScriptableObject
    {
        [SerializeField] private ChainEffectProcessor effectProcessor;
        public ChainEffectProcessor EffectProcessor => effectProcessor;
    }
}