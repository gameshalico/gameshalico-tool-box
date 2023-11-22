using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoEffectProcessor
{
    public interface IEffectProcessor
    {
        public UniTask Run(Func<CancellationToken, UniTask> function, CancellationToken cancellationToken = default);
    }
}