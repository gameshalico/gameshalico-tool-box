using System;
using UniRx;

namespace ShalicoToolBox
{
    public class DisposableContainer : IDisposable, IDisposableContainer
    {
        private readonly CompositeDisposable _disposable = new();

        public void Dispose()
        {
            _disposable.Dispose();
        }

        public void AddDisposable(IDisposable disposable)
        {
            _disposable.Add(disposable);
        }
    }

}