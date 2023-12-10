using System;
using System.Collections.Generic;

namespace ShalicoEffectProcessor.Context
{
    public class EffectContext
    {
        private static readonly Queue<EffectContext> s_pool = new();

        private readonly Dictionary<Type, ICloneable> _data = new();
        private int _refCount;

        private EffectContext()
        {
        }

        public EffectContext AddRef()
        {
            _refCount++;
            return this;
        }

        public static EffectContext Get()
        {
            if (s_pool.Count > 0)
                return s_pool.Dequeue();

            return new EffectContext();
        }

        public void Release()
        {
            if (_refCount > 0)
            {
                _refCount--;
                return;
            }

            if (_refCount < 0)
                throw new InvalidOperationException("Ref count is less than 0");

            _data.Clear();
            s_pool.Enqueue(this);
        }


        public void CopyFrom(EffectContext context)
        {
            foreach (var (key, value) in context._data)
                _data[key] = (ICloneable)value.Clone();
        }

        public EffectContext Clone()
        {
            var context = Get();
            context.CopyFrom(this);
            return context;
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