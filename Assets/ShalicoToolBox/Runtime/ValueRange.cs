using System;
using System.Collections.Generic;

namespace Shalico.ToolBox
{
    [Serializable]
    public struct ValueRange<T> where T : struct, IComparable<T>
    {
        public T min;
        public T max;

        public ValueRange(T min, T max)
        {
            this.min = min;
            this.max = max;
        }

        public ValueRange(ValueRange<T> other)
        {
            min = other.min;
            max = other.max;
        }

        private static T Min(T a, T b)
        {
            return a.CompareTo(b) < 0 ? a : b;
        }

        private static T Max(T a, T b)
        {
            return a.CompareTo(b) > 0 ? a : b;
        }


        public bool Contains(T value)
        {
            return min.CompareTo(value) <= 0 && value.CompareTo(max) <= 0;
        }

        public bool Contains(ValueRange<T> other)
        {
            return Contains(other.min) && Contains(other.max);
        }

        public bool Overlaps(ValueRange<T> other)
        {
            return Contains(other.min) || Contains(other.max);
        }

        public T Clamp(T value)
        {
            if (value.CompareTo(min) < 0)
            {
                return min;
            }

            if (value.CompareTo(max) > 0)
            {
                return max;
            }

            return value;
        }

        public ValueRange<T> Union(ValueRange<T> other)
        {
            (min, max) = Union(this, other);
            return this;
        }

        public ValueRange<T> Intersect(ValueRange<T> other)
        {
            (min, max) = Intersect(this, other);
            return this;
        }


        public ValueRange<T> Expand(T value)
        {
            (min, max) = Expand(this, value);
            return this;
        }

        /// <summary>
        ///     aとbの和となる範囲を返す
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ValueRange<T> Union(ValueRange<T> a, ValueRange<T> b)
        {
            return new ValueRange<T>(
                Min(a.min, b.min),
                Max(a.max, b.max)
            );
        }

        /// <summary>
        ///     aとbの積となる範囲を返す
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ValueRange<T> Intersect(ValueRange<T> a, ValueRange<T> b)
        {
            return new ValueRange<T>(
                Max(a.min, b.min),
                Min(a.max, b.max)
            );
        }

        /// <summary>
        ///     aからbを除外した範囲を返す
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static ValueRange<T>[] Except(ValueRange<T> a, ValueRange<T> b)
        {
            // 重なりがない場合
            if (!b.Overlaps(a))
            {
                return new[] { a };
            }

            // aがbに完全に含まれている場合
            if (b.Contains(a))
            {
                return Array.Empty<ValueRange<T>>();
            }

            // 重なっている場合
            List<ValueRange<T>> rangeList = new();
            if (a.min.CompareTo(b.min) < 0)
            {
                // a.min < b.min
                rangeList.Add(new ValueRange<T>(a.min, b.min));
            }

            if (b.max.CompareTo(a.max) < 0)
            {
                // b.max < a.max
                rangeList.Add(new ValueRange<T>(b.max, a.max));
            }

            return rangeList.ToArray();
        }

        public static ValueRange<T> Expand(ValueRange<T> range, T value)
        {
            return new ValueRange<T>(
                Min(range.min, value),
                Max(range.max, value)
            );
        }

        public static ValueRange<T> operator |(ValueRange<T> a, ValueRange<T> b)
        {
            return Union(a, b);
        }

        public static ValueRange<T> operator &(ValueRange<T> a, ValueRange<T> b)
        {
            return Intersect(a, b);
        }

        public static implicit operator ValueRange<T>((T min, T max) tuple)
        {
            return new ValueRange<T>(tuple.min, tuple.max);
        }

        public static implicit operator (T min, T max)(ValueRange<T> range)
        {
            return (range.min, range.max);
        }

        public bool IsEmpty()
        {
            return min.CompareTo(max) == 0;
        }

        public bool IsValid()
        {
            return min.CompareTo(max) <= 0;
        }

        public void Deconstruct(out T minValue, out T maxValue)
        {
            minValue = min;
            maxValue = max;
        }

        public override string ToString()
        {
            return $"[{min}, {max}]";
        }
    }
}