using System;
using System.Collections.Generic;
using UnityEngine;

namespace Shalico.ToolBox
{
    public static class ValueRangeExtensions
    {
        public static ValueRange<T> ToValueRange<T>(this RangeInt range) where T : struct, IComparable<T>, IConvertible
        {
            return new ValueRange<T>((T)Convert.ChangeType(range.start, typeof(T)),
                (T)Convert.ChangeType(range.end, typeof(T)));
        }

        public static double Remap<T>(this ValueRange<T> range, T value) where T : struct, IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = value.ToDouble(null);
            return (doubleValue - doubleMin) / (doubleMax - doubleMin);
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

        public static ValueRange<T> Shifted<T>(this ValueRange<T> range, T value)
            where T : struct, IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = value.ToDouble(null);
            double doubleShift = doubleValue - doubleMin;
            return new ValueRange<T>((T)Convert.ChangeType(doubleMin + doubleShift, typeof(T)),
                (T)Convert.ChangeType(doubleMax + doubleShift, typeof(T)));
        }

        public static ValueRange<T> Shift<T>(ref this ValueRange<T> range, T value)
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

        public static IEnumerator<int> GetEnumerator(this ValueRange<int> range)
        {
            for (int i = range.min; i < range.max; i++)
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