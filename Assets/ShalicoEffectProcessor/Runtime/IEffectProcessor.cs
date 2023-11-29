using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoEffectProcessor.Context;

namespace ShalicoEffectProcessor
{
    public delegate UniTask EffectFunc(EffectContext context, CancellationToken cancellationToken);


    public interface IEffectProcessor
    {
        public UniTask Run(EffectContext context, EffectFunc function,
            CancellationToken cancellationToken = default);
    }
}