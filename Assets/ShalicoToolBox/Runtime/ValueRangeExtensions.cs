using System;
using System.Collections.Generic;
using UnityEngine;

namespace ShalicoToolBox
{
    public static class ValueRangeExtensions
    {
        public static ValueRange<T> ToValueRange<T>(this RangeInt range) where T : struct, IComparable<T>, IConvertible
        {
            return new ValueRange<T>((T)Convert.ChangeType(range.start, typeof(T)),
                (T)Convert.ChangeType(range.end, typeof(T)));
        }

        public static double Remap<T>(this ValueRange<T> range, ValueRange<T> from, T value)
            where T : struct, IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = value.ToDouble(null);
            double doubleFromMin = from.min.ToDouble(null);
            double doubleFromMax = from.max.ToDouble(null);
            return doubleMin + ((doubleValue - doubleFromMin) / (doubleFromMax - doubleFromMin) *
                                (doubleMax - doubleMin));
        }

        public static T Lerp<T>(this ValueRange<T> range, double t) where T : struct, IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = doubleMin + ((doubleMax - doubleMin) * t);
            return (T)Convert.ChangeType(doubleValue, typeof(T));
        }

        public static T InverseLerp<T>(this ValueRange<T> range, double t)
            where T : struct, IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = doubleMin + ((doubleMax - doubleMin) * t);
            return (T)Convert.ChangeType((doubleValue - doubleMin) / (doubleMax - doubleMin), typeof(T));
        }

        public static T Random<T>(this ValueRange<T> range) where T : struct, IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = UnityEngine.Random.Range((float)doubleMin, (float)doubleMax);
            return (T)Convert.ChangeType(doubleValue, typeof(T));
        }

        public static T Median<T>(this ValueRange<T> range) where T : struct, IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = doubleMin + ((doubleMax - doubleMin) * 0.5);
            return (T)Convert.ChangeType(doubleValue, typeof(T));
        }

        public static ValueRange<T> Shifted<T>(this ValueRange<T> range, T shift)
            where T : struct, IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleShift = shift.ToDouble(null);
            return new ValueRange<T>((T)Convert.ChangeType(doubleMin + doubleShift, typeof(T)),
                (T)Convert.ChangeType(doubleMax + doubleShift, typeof(T)));
        }

        public static ValueRange<T> Shift<T>(this ValueRange<T> range, T value)
            where T : struct, IComparable<T>, IConvertible
        {
            (range.min, range.max) = range.Shifted(value);
            return range;
        }

        public static ValueRange<T>[] Split<T>(this ValueRange<T> range, int count)
            where T : struct, IComparable<T>, IConvertible
        {
            ValueRange<T>[] result = new ValueRange<T>[count];
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleStep = (doubleMax - doubleMin) / count;
            for (int i = 0; i < count; i++)
            {
                double doubleValue = doubleMin + (doubleStep * i);
                result[i] = new ValueRange<T>((T)Convert.ChangeType(doubleValue, typeof(T)),
                    (T)Convert.ChangeType(doubleValue + doubleStep, typeof(T)));
            }

            return result;
        }

        public static T Length<T>(this ValueRange<T> range)
            where T : struct, IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            return (T)Convert.ChangeType(doubleMax - doubleMin, typeof(T));
        }

        public static IEnumerable<T> Enumerate<T>(this ValueRange<T> range, T step)
            where T : struct, IComparable<T>, IConvertible
        {
            double doubleValue = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleStep = step.ToDouble(null);
            while (doubleValue < doubleMax)
            {
                yield return (T)Convert.ChangeType(doubleValue, typeof(T));
                doubleValue += doubleStep;
            }
        }

        public static int Length(this ValueRange<int> range)
        {
            return range.max - range.min;
        }

        public static IEnumerable<int> Enumerate(this ValueRange<int> range, int step = 1)
        {
            for (int i = range.min; i < range.max; i += step)
            {
                yield return i;
            }
        }

        public static ValueRange<T>[] Split<T>(this ValueRange<T> range, T step)
            where T : struct, IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleStep = step.ToDouble(null);
            int count = (int)((doubleMax - doubleMin) / doubleStep);
            ValueRange<T>[] result = new ValueRange<T>[count];
            for (int i = 0; i < count; i++)
            {
                double doubleValue = doubleMin + (doubleStep * i);
                result[i] = new ValueRange<T>((T)Convert.ChangeType(doubleValue, typeof(T)),
                    (T)Convert.ChangeType(doubleValue + doubleStep, typeof(T)));
            }

            return result;
        }
    }
}