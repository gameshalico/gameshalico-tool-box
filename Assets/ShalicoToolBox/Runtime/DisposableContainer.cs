using System;
using System.Threading;
using UniRx;

namespace ShalicoToolBox
{
    public class DisposableContainer : IDisposable, IDisposableContainer
    {
        private readonly CancellationTokenSource _cancellationTokenSource = new();
        private readonly CompositeDisposable _disposables = new();

        public void Dispose()
        {
            _disposables.Dispose();
            _cancellationTokenSource.Cancel();
        }

        public void AddDisposable(IDisposable disposable)
        {
            _disposables.Add(disposable);
        }

        public CancellationToken GetCancellationTokenOnDispose()
        {
            return _cancellationTokenSource.Token;
        }
    }

}