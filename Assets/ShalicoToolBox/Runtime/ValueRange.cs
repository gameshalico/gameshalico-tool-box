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
            return Mathf.Clamp(value, Min, Max);
        }

        public float Remap(float value, ValueRange from)
        {
            return Remap(value, from, this);
        }

        public static float Remap(float value, ValueRange from, ValueRange to)
        {
            return Mathf.Lerp(to.Min, to.Max, Mathf.InverseLerp(from.Min, from.Max, value));
        }
    }
}