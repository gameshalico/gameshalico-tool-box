using System;
using System.Collections.Generic;

namespace ShalicoToolBox
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

        /// <summary>
        ///     valueが範囲内にあるか判定する
        /// </summary>
        public bool Contains(T value)
        {
            return min.CompareTo(value) <= 0 && value.CompareTo(max) <= 0;
        }

        /// <summary>
        ///     otherの範囲を包含するか判定する
        /// </summary>
        public bool Contains(ValueRange<T> other)
        {
            return min.CompareTo(other.min) <= 0 && other.max.CompareTo(max) <= 0;
        }

        /// <summary>
        ///     otherと重なるか判定する
        /// </summary>
        public bool Overlaps(ValueRange<T> other)
        {
            return min.CompareTo(other.max) <= 0 && other.min.CompareTo(max) <= 0;
        }

        /// <summary>
        ///     valueを範囲内に収める
        /// </summary>
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

        /// <summary>
        ///     otherと合成した範囲を返す(和)
        /// </summary>
        public ValueRange<T> Union(ValueRange<T> other)
        {
            return Union(this, other);
        }

        /// <summary>
        ///     otherと重なる範囲を返す(積)
        /// </summary>
        public ValueRange<T> Intersect(ValueRange<T> other)
        {
            return Intersect(this, other);
        }

        /// <summary>
        ///     otherの範囲を除外した範囲を返す
        /// </summary>
        public ValueRange<T>[] Subtract(ValueRange<T> other)
        {
            return Subtract(this, other);
        }


        /// <summary>
        ///     valueを含むように範囲を拡張する
        /// </summary>
        public ValueRange<T> Expand(T value)
        {
            return Expand(this, value);
        }

        /// <summary>
        ///     aとbの和となる範囲を返す
        /// </summary>
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
        public static ValueRange<T> Intersect(ValueRange<T> a, ValueRange<T> b)
        {
            ValueRange<T> result = new(
                Max(a.min, b.min),
                Min(a.max, b.max)
            );

            if (!result.IsValid())
            {
                result = new ValueRange<T>();
            }

            return result;
        }

        /// <summary>
        ///     aからbを除外した範囲を返す
        /// </summary>
        public static ValueRange<T>[] Subtract(ValueRange<T> a, ValueRange<T> b)
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
            if (a.min.CompareTo(b.min) < 0) // a.min < b.min
            {
                rangeList.Add(new ValueRange<T>(a.min, b.min));
            }

            if (b.max.CompareTo(a.max) < 0) // b.max < a.max
            {
                rangeList.Add(new ValueRange<T>(b.max, a.max));
            }

            return rangeList.ToArray();
        }

        /// <summary>
        ///     valueを含むように範囲を拡張する
        /// </summary>
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

        /// <summary>
        ///     min=maxか判定する
        /// </summary>
        public bool IsEmpty()
        {
            return min.CompareTo(max) == 0;
        }

        /// <summary>
        ///     minがmax未満であるか判定する
        /// </summary>
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

        public static bool operator ==(ValueRange<T> a, ValueRange<T> b)
        {
            return a.min.Equals(b.min) && a.max.Equals(b.max);
        }

        public static bool operator !=(ValueRange<T> a, ValueRange<T> b)
        {
            return !(a == b);
        }

        public override bool Equals(object obj)
        {
            if (obj is ValueRange<T> other)
            {
                return this == other;
            }

            return false;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(min, max);
        }
    }
}