using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoFunctionRunner
{
    public interface IEffect
    {
        public UniTask PlayEffectAsync(CancellationToken cancellationToken = default);
    }
}