using System;
using System.Collections.Generic;

namespace ShalicoEffectProcessor.Context
{
    public class EffectContext
    {
        private readonly Dictionary<Type, ICloneable> _data = new();

        public EffectContext()
        {
        }

        public EffectContext(EffectContext context)
        {
            foreach (var (key, value) in context._data)
                _data[key] = (ICloneable)value.Clone();
        }

        public EffectContext Clone()
        {
            return new EffectContext(this);
        }

        public EffectContext CloneIf(bool condition)
        {
            return condition ? new EffectContext(this) : this;
        }

        public void Set<T>(T data) where T : ICloneable
        {
            _data[typeof(T)] = data;
        }

        public T Get<T>()
        {
            if (_data.TryGetValue(typeof(T), out var data))
                return (T)data;

            throw new KeyNotFoundException($"EffectContext does not contain {typeof(T)}");
        }

        public T GetOrDefault<T>(T defaultValue = default)
        {
            if (_data.TryGetValue(typeof(T), out var data))
                return (T)data;

            return defaultValue;
        }

        public bool TryGet<T>(out T data)
        {
            if (_data.TryGetValue(typeof(T), out var d))
            {
                data = (T)d;
                return true;
            }

            data = default;
            return false;
        }
    }
}