using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffectProcessor.Context;

namespace ShalicoEffectProcessor
{
    public abstract class ImmediateEffect : IEffect
    {
        public UniTask PlayEffectAsync(EffectContext context, CancellationToken cancellationToken)
        {
            PlayEffectImmediate(context);
            return UniTask.CompletedTask;
        }

        protected abstract void PlayEffectImmediate(EffectContext context);
    }
}