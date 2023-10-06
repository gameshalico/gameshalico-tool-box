using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoToolBox
{
    public static class EnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> sequence, Action<T> action)
        {
            foreach (T item in sequence)
            {
                action(item);
            }
        }

        public static async UniTask Sequence<T>(this IEnumerable<T> sequence, Func<T, UniTask> action)
        {
            foreach (T item in sequence)
            {
                await action(item);
            }
        }

        public static void ForEachAsync<T>(this IEnumerable<T> sequence,
            Func<T, CancellationToken, UniTask> action, CancellationToken token)
        {
            foreach (T item in sequence)
            {
                action(item, token).Forget();
            }
        }

        public static void ForEachAsync<T>(this IEnumerable<T> sequence, Func<T, UniTask> action)
        {
            foreach (T item in sequence)
            {
                action(item).Forget();
            }
        }
    }
}