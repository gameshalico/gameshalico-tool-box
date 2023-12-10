using System;

namespace ShalicoToolBox
{
    public static class DisposableContainerExtensions
    {
        public static T AddTo<T>(this T disposable, IDisposableContainer container) where T : IDisposable
        {
            container.AddDisposable(disposable);
            return disposable;
        }
    }
}