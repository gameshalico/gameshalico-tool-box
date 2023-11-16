using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffect.FunctionRunners;
using UnityEngine;

namespace ShalicoEffect.Effects
{
    public class FunctionRunnerHolder : MonoBehaviour
    {
        [SerializeField] private ChainFunctionRunner chainFunctionRunner;

        public async UniTask RunAsync(CancellationToken cancellationToken)
        {
            await chainFunctionRunner.RunAsync(cancellationToken);
        }
    }
}