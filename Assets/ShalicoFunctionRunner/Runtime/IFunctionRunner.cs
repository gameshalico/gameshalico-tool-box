using System;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoFunctionRunner
{
    public interface IFunctionRunner
    {
        public UniTask Run(Func<CancellationToken, UniTask> function, CancellationToken cancellationToken = default);
    }
}