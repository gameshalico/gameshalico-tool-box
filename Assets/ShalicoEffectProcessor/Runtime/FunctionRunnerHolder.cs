using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffectProcessor.EffectProcessors;
using UnityEngine;

namespace ShalicoEffectProcessor
{
    public class EffectProcessorHolder : MonoBehaviour
    {
        [SerializeField] private ChainEffectProcessor chainEffectProcessor;

        public async UniTask RunAsync(CancellationToken cancellationToken)
        {
            await chainEffectProcessor.RunAsync(cancellationToken);
        }
    }
}