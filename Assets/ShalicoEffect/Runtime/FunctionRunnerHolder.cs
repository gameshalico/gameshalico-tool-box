using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffect.FunctionRunners;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShalicoEffect.Effects
{
    public class FunctionRunnerHolder : MonoBehaviour
    {
        [FormerlySerializedAs("functionRunnner")] [SerializeField]
        private FunctionRunnerChain functionRunner;

        public virtual async UniTask PlayAsync(CancellationToken cancellationToken)
        {
            await functionRunner.RunAsync(cancellationToken);
        }
    }
}