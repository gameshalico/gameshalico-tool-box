using System;
using System.Collections.Generic;

namespace ShalicoEffectProcessor.Context
{
    public class EffectContext
    {
        private static readonly Queue<EffectContext> s_pool = new();

        private readonly Dictionary<Type, IContextItem> _data = new();
        private int _refCount;

        private EffectContext()
        {
        }

        /// <summary>
        ///     Add reference to this context. EffectContext must be released again after use with Release().
        /// </summary>
        /// <returns> self EffectContext </returns>
        public EffectContext AddRef()
        {
            _refCount++;
            return this;
        }

        /// <summary>
        ///     Get a new EffectContext from the pool. EffectContext must be released after use with Release().
        /// </summary>
        /// <returns> new EffectContext </returns>
        public static EffectContext Rent()
        {
            if (s_pool.Count > 0)
                return s_pool.Dequeue();

            return new EffectContext();
        }

        /// <summary>
        ///     Release this EffectContext. If ref count is 0, it will be returned to the pool.
        /// </summary>
        /// <exception cref="InvalidOperationException"> Ref count is less than 0 </exception>
        public void Release()
        {
            if (_refCount > 0)
            {
                _refCount--;
                return;
            }

            if (_refCount < 0)
                throw new InvalidOperationException("Ref count is less than 0");

            foreach (var item in _data.Values) item.OnRelease();
            _data.Clear();
            s_pool.Enqueue(this);
        }


        /// <summary>
        ///     Copy data from another EffectContext.
        /// </summary>
        /// <param name="context"> EffectContext to copy from </param>
        public void CopyFrom(EffectContext context)
        {
            foreach (var (key, value) in context._data)
                _data[key] = (IContextItem)value.Clone();
        }

        /// <summary>
        ///     Clone this EffectContext. EffectContext must be released after use with Release().
        /// </summary>
        /// <returns> cloned EffectContext </returns>
        public EffectContext Clone()
        {
            var context = Rent();
            context.CopyFrom(this);
            return context;
        }

        public EffectContext Merge(EffectContext context, bool overwrite = false)
        {
            foreach (var (key, value) in context._data)
                if (overwrite || !_data.ContainsKey(key))
                    _data[key] = value;

            return this;
        }

        /// <summary>
        ///     Set data in this EffectContext.
        /// </summary>
        /// <param name="data"> data to set </param>
        /// <typeparam name="T"> type of data </typeparam>
        public void Set<T>(T data) where T : IContextItem
        {
            _data[typeof(T)] = data;
        }

        public void Set(Type type, IContextItem data)
        {
            _data[type] = data;
        }

        /// <summary>
        ///     Get data from this EffectContext.
        /// </summary>
        /// <typeparam name="T"> type of data </typeparam>
        /// <returns> data </returns>
        /// <exception cref="KeyNotFoundException"> EffectContext does not contain data of type T </exception>
        public T Get<T>()
        {
            if (_data.TryGetValue(typeof(T), out var data))
                return (T)data;

            throw new KeyNotFoundException($"EffectContext does not contain {typeof(T)}");
        }

        public IContextItem Get(Type type)
        {
            if (_data.TryGetValue(type, out var data))
                return data;

            throw new KeyNotFoundException($"EffectContext does not contain {type}");
        }

        /// <summary>
        ///     Try to get data from this EffectContext.
        /// </summary>
        /// <param name="data"> data </param>
        /// <typeparam name="T"> type of data </typeparam>
        /// <returns> true if data was found, false otherwise </returns>
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

        public bool TryGet(Type type, out IContextItem data)
        {
            if (_data.TryGetValue(type, out var d))
            {
                data = d;
                return true;
            }

            data = default;
            return false;
        }
    }
}