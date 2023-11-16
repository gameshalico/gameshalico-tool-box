using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoFunctionRunner.FunctionRunners;
using UnityEngine;

namespace ShalicoFunctionRunner
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