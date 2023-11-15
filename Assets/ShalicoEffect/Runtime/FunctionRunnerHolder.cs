using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffect.FunctionRunners;
using UnityEngine;

namespace ShalicoEffect.Effects
{
    public class FunctionRunnerHolder : MonoBehaviour
    {
        [SerializeField] private FunctionRunnerChain functionRunner;

        public virtual async UniTask PlayAsync(CancellationToken cancellationToken)
        {
            await functionRunner.RunAsync(cancellationToken);
        }
    }
}