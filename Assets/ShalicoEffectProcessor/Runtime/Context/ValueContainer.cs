using System.Collections.Generic;

namespace ShalicoEffectProcessor.Context
{
    public class ValueContainer<T> : IContextItem
    {
        private static readonly Stack<ValueContainer<T>> s_pool = new();

        private ValueContainer(T value)
        {
            Value = value;
        }

        public T Value { get; set; }

        public void OnRelease()
        {
            Release(this);
        }

        public object Clone()
        {
            return Get(Value);
        }

        public static ValueContainer<T> Get(T value)
        {
            if (s_pool.Count > 0)
            {
                var container = s_pool.Pop();
                container.Value = value;
                return container;
            }

            return new ValueContainer<T>(value);
        }

        public static void Release(ValueContainer<T> container)
        {
            s_pool.Push(container);
        }
    }
}