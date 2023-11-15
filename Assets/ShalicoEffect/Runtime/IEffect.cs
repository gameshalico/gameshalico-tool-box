using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoEffect
{
    public interface IEffect
    {
        public UniTask PlayEffectAsync(CancellationToken cancellationToken = default);
    }
}