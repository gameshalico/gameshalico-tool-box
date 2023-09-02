using System;
using UnityEngine;

namespace Shalico.ToolBox
{
    public static class ValueRangeExtensions
    {
        public static ValueRange<T> ToValueRange<T>(this RangeInt range) where T : IComparable<T>, IConvertible
        {
            return new ValueRange<T>((T)Convert.ChangeType(range.start, typeof(T)),
                (T)Convert.ChangeType(range.end, typeof(T)));
        }

        public static double Remap<T>(this ValueRange<T> range, T value) where T : IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = value.ToDouble(null);
            return (doubleValue - doubleMin) / (doubleMax - doubleMin);
        }

        public static T Lerp<T>(this ValueRange<T> range, double t) where T : IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = doubleMin + ((doubleMax - doubleMin) * t);
            return (T)Convert.ChangeType(doubleValue, typeof(T));
        }

        public static T InverseLerp<T>(this ValueRange<T> range, double t) where T : IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = doubleMin + ((doubleMax - doubleMin) * t);
            return (T)Convert.ChangeType((doubleValue - doubleMin) / (doubleMax - doubleMin), typeof(T));
        }

        public static T Random<T>(this ValueRange<T> range) where T : IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = UnityEngine.Random.Range((float)doubleMin, (float)doubleMax);
            return (T)Convert.ChangeType(doubleValue, typeof(T));
        }

        public static T Median<T>(this ValueRange<T> range) where T : IComparable<T>, IConvertible
        {
            double doubleMin = range.min.ToDouble(null);
            double doubleMax = range.max.ToDouble(null);
            double doubleValue = doubleMin + ((doubleMax - doubleMin) * 0.5);
            return (T)Convert.ChangeType(doubleValue, typeof(T));
        }

        public static ValueRange<T>[] Split<T>(this ValueRange<T> range, int count)
            where T : IComparable<T>, IConvertible
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

        public static ValueRange<T>[] Split<T>(this ValueRange<T> range, T step)
            where T : IComparable<T>, IConvertible
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