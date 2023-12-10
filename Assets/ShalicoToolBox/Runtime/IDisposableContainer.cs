using System;

namespace ShalicoToolBox
{
    public interface IDisposableContainer
    {
        void AddDisposable(IDisposable disposable);
    }
}