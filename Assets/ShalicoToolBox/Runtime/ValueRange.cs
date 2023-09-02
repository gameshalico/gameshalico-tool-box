using System;

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

        public ValueRange<T> Except(ValueRange<T> other)
        {
            (min, max) = Except(this, other);
            return this;
        }

        public ValueRange<T> Expand(T value)
        {
            (min, max) = Expand(this, value);
            return this;
        }

        public static ValueRange<T> Union(ValueRange<T> a, ValueRange<T> b)
        {
            return new ValueRange<T>(
                a.min.CompareTo(b.min) < 0 ? a.min : b.min,
                a.max.CompareTo(b.max) > 0 ? a.max : b.max
            );
        }

        public static ValueRange<T> Intersect(ValueRange<T> a, ValueRange<T> b)
        {
            return new ValueRange<T>(
                a.min.CompareTo(b.min) > 0 ? a.min : b.min,
                a.max.CompareTo(b.max) < 0 ? a.max : b.max
            );
        }

        public static ValueRange<T> Except(ValueRange<T> a, ValueRange<T> b)
        {
            return new ValueRange<T>(
                a.min.CompareTo(b.min) > 0 ? a.min : b.min,
                a.max.CompareTo(b.max) < 0 ? a.max : b.max
            );
        }

        public static ValueRange<T>[] Subtract(ValueRange<T> a, ValueRange<T> b)
        {
            if (b.Contains(a))
            {
                return Array.Empty<ValueRange<T>>();
            }

            if (a.Contains(b))
            {
                return new[] { new ValueRange<T>(a.min, b.min), new ValueRange<T>(b.max, a.max) };
            }

            return new[] { Except(a, b) };
        }

        public static ValueRange<T> Expand(ValueRange<T> range, T value)
        {
            return new ValueRange<T>(
                range.min.CompareTo(value) < 0 ? range.min : value,
                range.max.CompareTo(value) > 0 ? range.max : value
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
    }
}