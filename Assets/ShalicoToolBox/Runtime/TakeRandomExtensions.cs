using System;
using System.Collections.Generic;
using System.Linq;
using Random = UnityEngine.Random;

namespace ShalicoToolBox
{
    public static class TakeRandomExtensions
    {
        public static T TakeRandom<T>(this IReadOnlyList<T> items)
        {
            var randomIndex = Random.Range(0, items.Count);

            return items[randomIndex];
        }

        public static T[] TakeRandom<T>(this IReadOnlyList<T> items, int count)
        {
            var result = new T[count];
            var itemsCount = items.Count;
            for (var i = 0; i < count; i++)
            {
                var randomIndex = Random.Range(0, itemsCount);
                result[i] = items[randomIndex];
            }

            return result;
        }

        public static T TakeRandom<T>(this T[] items)
        {
            var randomIndex = Random.Range(0, items.Length);

            return items[randomIndex];
        }

        public static T[] TakeRandom<T>(this T[] items, int count)
        {
            var result = new T[count];
            var itemsCount = items.Length;
            for (var i = 0; i < count; i++)
            {
                var randomIndex = Random.Range(0, itemsCount);
                result[i] = items[randomIndex];
            }

            return result;
        }

        public static T[] TakeRandomDistinct<T>(this IEnumerable<T> items, int count)
        {
            var itemArray = items.ToArray();
            if (count < 0 || count > itemArray.Length) throw new ArgumentOutOfRangeException(nameof(count));

            var result = new T[count];
            for (var i = 0; i < count; i++)
            {
                var randomIndex = Random.Range(i, itemArray.Length);
                (itemArray[i], itemArray[randomIndex]) = (itemArray[randomIndex], itemArray[i]);
                result[i] = itemArray[i];
            }

            return result;
        }
    }
}