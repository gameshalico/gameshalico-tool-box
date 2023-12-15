using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffectProcessor.Context;

namespace ShalicoEffectProcessor.EffectProcessors
{
    public abstract class UniformEffectProcessor : IEffectProcessor
    {
        public async UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default)
        {
            await Run(context, cancellationToken);
            await function(context, cancellationToken);
        }

        protected abstract UniTask Run(EffectContext context, CancellationToken cancellationToken);
    }
}