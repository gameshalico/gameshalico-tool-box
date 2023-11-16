using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoFunctionRunner
{
    public static class FunctionRunnerExtensions
    {
        public static void Run(this IFunctionRunner functionRunner, CancellationToken cancellationToken = default)
        {
            functionRunner.Run(_ => UniTask.CompletedTask, cancellationToken);
        }

        public static async UniTask RunAsync(this IFunctionRunner functionRunner,
            CancellationToken cancellationToken = default)
        {
            await functionRunner.Run(_ => UniTask.CompletedTask, cancellationToken);
        }
    }
}