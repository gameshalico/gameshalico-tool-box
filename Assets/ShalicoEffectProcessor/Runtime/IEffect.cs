using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffectProcessor.Context;

namespace ShalicoEffectProcessor
{
    public interface IEffect
    {
        public UniTask PlayEffectAsync(EffectContext context, CancellationToken cancellationToken = default);
    }
}