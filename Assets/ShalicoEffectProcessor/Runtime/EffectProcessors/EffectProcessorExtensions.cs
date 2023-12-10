using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffectProcessor.Context;

namespace ShalicoEffectProcessor.EffectProcessors
{
    public static class EffectProcessorExtensions
    {
        public static void Run(this IEffectProcessor effectProcessor, CancellationToken cancellationToken = default)
        {
            effectProcessor.RunAsync(cancellationToken).Forget();
        }

        public static async UniTask RunAsync(this IEffectProcessor effectProcessor,
            CancellationToken cancellationToken = default)
        {
            await effectProcessor.RunAsync(EffectContext.Get(), cancellationToken);
        }

        public static void Run(this IEffectProcessor effectProcessor, EffectContext context,
            CancellationToken cancellationToken = default)
        {
            effectProcessor.RunAsync(context, cancellationToken).Forget();
        }

        public static async UniTask RunAsync(this IEffectProcessor effectProcessor, EffectContext context,
            CancellationToken cancellationToken = default)
        {
            try
            {
                await effectProcessor.Run(context, (_, _) => UniTask.CompletedTask, cancellationToken);
            }
            finally
            {
                context.Release();
            }
        }
    }
}