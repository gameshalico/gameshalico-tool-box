using System;
using UnityEngine;

namespace ShalicoToolBox
{
    [Serializable]
    public struct ValueRange<T> where T : struct, IComparable<T>
    {
        [SerializeField] private T min;
        [SerializeField] private T max;

        public ValueRange(T min, T max)
        {
            this.min = min;
            this.max = max;

            if (min.CompareTo(max) > 0)
            {
                throw new ArgumentException("min must be less than or equal to max");
            }
        }

        public ValueRange(ValueRange<T> other)
        {
            min = other.min;
            max = other.max;
        }

        public T Min => min;
        public T Max => max;

        private static T EvaluateMin(T a, T b)
        {
            return a.CompareTo(b) < 0 ? a : b;
        }

        private static T EvaluateMax(T a, T b)
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
                EvaluateMin(a.min, b.min),
                EvaluateMax(a.max, b.max)
            );
        }

        /// <summary>
        ///     aとbの積となる範囲を返す
        /// </summary>
        public static ValueRange<T> Intersect(ValueRange<T> a, ValueRange<T> b)
        {
            T max = EvaluateMin(a.max, b.max);
            T min = EvaluateMax(a.min, b.min);

            if (max.CompareTo(min) < 0)
            {
                return new ValueRange<T>(default, default);
            }

            return new ValueRange<T>(min, max);
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

            // 重なっているか、bがaに完全に含まれている場合
            ValueRange<T>[] rangeArray = new ValueRange<T>[2];
            int count = 0;
            if (a.min.CompareTo(b.min) < 0) // a.min < b.min
            {
                rangeArray[count++] = new ValueRange<T>(a.min, b.min);
            }

            if (b.max.CompareTo(a.max) < 0) // b.max < a.max
            {
                rangeArray[count++] = new ValueRange<T>(b.max, a.max);
            }

            ValueRange<T>[] result = new ValueRange<T>[count];
            Array.Copy(rangeArray, result, count);

            return result;
        }

        /// <summary>
        ///     valueを含むように範囲を拡張する
        /// </summary>
        public static ValueRange<T> Expand(ValueRange<T> range, T value)
        {
            return new ValueRange<T>(
                EvaluateMin(range.min, value),
                EvaluateMax(range.max, value)
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