using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffect.FunctionRunners;
using UnityEngine;
using UnityEngine.Serialization;

namespace ShalicoEffect.Effects
{
    public class FunctionRunnerHolder : MonoBehaviour
    {
        [FormerlySerializedAs("functionRunner")] [SerializeField]
        private ChainFunctionRunner chainFunctionRunner;

        public virtual async UniTask PlayAsync(CancellationToken cancellationToken)
        {
            await chainFunctionRunner.RunAsync(cancellationToken);
        }
    }
}