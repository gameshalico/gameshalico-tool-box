using System;
using UnityEngine;

namespace Shalico.ToolBox
{
    [Serializable]
    public struct ValueRange
    {
        public static readonly ValueRange ZeroToOne = new(0, 1);
        public static readonly ValueRange MinusOneToOne = new(-1, 1);

        [SerializeField] private float max;
        [SerializeField] private float min;

        public ValueRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }

        public float Min
        {
            get => min;
            set => min = value;
        }

        public float Max
        {
            get => max;
            set => max = value;
        }

        public bool Contains(float value)
        {
            return Min <= value && value <= Max;
        }

        public float Clamp(float value)
        {
            return Clamp(value, this);
        }

        public float Remap(float value, ValueRange from)
        {
            return Remap(value, from, this);
        }

        public static bool Contains(float value, ValueRange range)
        {
            return range.Min <= value && value <= range.Max;
        }

        public static float Clamp(float value, ValueRange range)
        {
            return Mathf.Clamp(value, range.Min, range.Max);
        }

        public static float Remap(float value, ValueRange from, ValueRange to)
        {
            return Mathf.Lerp(to.Min, to.Max, Mathf.InverseLerp(from.Min, from.Max, value));
        }


        public static implicit operator ValueRange((float min, float max) tuple)
        {
            return new ValueRange(tuple.min, tuple.max);
        }

        public static implicit operator (float min, float max)(ValueRange range)
        {
            return (range.Min, range.Max);
        }

        public float Random()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}