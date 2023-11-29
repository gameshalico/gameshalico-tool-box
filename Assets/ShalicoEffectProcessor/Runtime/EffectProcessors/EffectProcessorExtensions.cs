using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffectProcessor.Context;

namespace ShalicoEffectProcessor.EffectProcessors
{
    public static class EffectProcessorExtensions
    {
        public static void Run(this IEffectProcessor effectProcessor, CancellationToken cancellationToken = default)
        {
            effectProcessor.Run(new EffectContext(), (_, _) => UniTask.CompletedTask, cancellationToken);
        }

        public static async UniTask RunAsync(this IEffectProcessor effectProcessor,
            CancellationToken cancellationToken = default)
        {
            await effectProcessor.Run(new EffectContext(), (_, _) => UniTask.CompletedTask, cancellationToken);
        }
    }
}