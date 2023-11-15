using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoEffect
{
    public abstract class ImmediateEffect : Effect
    {
        protected override UniTask PlayEffectWithTimingAsync(CancellationToken token)
        {
            PlayEffectImmediate();
            return UniTask.CompletedTask;
        }

        protected abstract void PlayEffectImmediate();
    }
}