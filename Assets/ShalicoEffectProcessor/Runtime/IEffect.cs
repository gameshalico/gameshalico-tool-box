using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoEffectProcessor
{
    public interface IEffect
    {
        public UniTask PlayEffectAsync(CancellationToken cancellationToken = default);
    }
}