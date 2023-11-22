using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoEffectProcessor
{
    public abstract class ImmediateEffect : IEffect
    {
        public UniTask PlayEffectAsync(CancellationToken cancellationToken)
        {
            PlayEffectImmediate();
            return UniTask.CompletedTask;
        }

        protected abstract void PlayEffectImmediate();
    }
}