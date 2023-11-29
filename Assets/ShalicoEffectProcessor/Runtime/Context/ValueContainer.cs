using System;

namespace ShalicoEffectProcessor.Context
{
    public class ValueContainer<T> : ICloneable where T : struct
    {
        public ValueContainer()
        {
        }

        public ValueContainer(T value)
        {
            Value = value;
        }

        public ValueContainer(ValueContainer<T> value)
        {
            Value = value.Value;
        }

        public T Value { get; set; }

        public object Clone()
        {
            return new ValueContainer<T>(this);
        }
    }
}