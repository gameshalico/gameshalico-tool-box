using System;
using UnityEngine;

namespace Shalico.ToolBox
{
    [Serializable]
    public class ValueRangeFloatController
    {
        public static readonly ValueRangeFloatController ZeroToOne = new(0, 1);
        public static readonly ValueRangeFloatController MinusOneToOne = new(-1, 1);

        [SerializeField] private float max;
        [SerializeField] private float min;

        public ValueRangeFloatController(float min, float max)
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

        public float Remap(float value, ValueRangeFloatController from)
        {
            return Remap(value, from, this);
        }

        public static bool Contains(float value, ValueRangeFloatController rangeFloatController)
        {
            return rangeFloatController.Min <= value && value <= rangeFloatController.Max;
        }

        public static float Clamp(float value, ValueRangeFloatController rangeFloatController)
        {
            return Mathf.Clamp(value, rangeFloatController.Min, rangeFloatController.Max);
        }

        public static float Remap(float value, ValueRangeFloatController from, ValueRangeFloatController to)
        {
            return Mathf.Lerp(to.Min, to.Max, Mathf.InverseLerp(from.Min, from.Max, value));
        }


        public static implicit operator ValueRangeFloatController((float min, float max) tuple)
        {
            return new ValueRangeFloatController(tuple.min, tuple.max);
        }

        public static implicit operator (float min, float max)(ValueRangeFloatController rangeFloatController)
        {
            return (rangeFloatController.Min, rangeFloatController.Max);
        }

        public float Random()
        {
            return UnityEngine.Random.Range(Min, Max);
        }
    }
}