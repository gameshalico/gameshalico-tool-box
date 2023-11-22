using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoEffectProcessor
{
    public static class EffectProcessorExtensions
    {
        public static void Run(this IEffectProcessor EffectProcessor, CancellationToken cancellationToken = default)
        {
            EffectProcessor.Run(_ => UniTask.CompletedTask, cancellationToken);
        }

        public static async UniTask RunAsync(this IEffectProcessor EffectProcessor,
            CancellationToken cancellationToken = default)
        {
            await EffectProcessor.Run(_ => UniTask.CompletedTask, cancellationToken);
        }
    }
}