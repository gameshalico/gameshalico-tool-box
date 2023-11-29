using System.Collections.Generic;

namespace ShalicoEffectProcessor.ContextObjects
{
    public static class ValueContainerExtensions
    {
        public static T GetValue<T>(this EffectContext context) where T : struct
        {
            if (context.TryGet(out ValueContainer<T> container))
                return container.Value;

            throw new KeyNotFoundException($"EffectContext does not contain {typeof(T)}");
        }

        public static bool TryGetValue<T>(this EffectContext context, out T value) where T : struct
        {
            if (context.TryGet(out ValueContainer<T> container))
            {
                value = container.Value;
                return true;
            }

            value = default;
            return false;
        }

        public static T SetValue<T>(this EffectContext context, T value) where T : struct
        {
            if (context.TryGet(out ValueContainer<T> container))
                container.Value = value;
            else
                context.Set(new ValueContainer<T>(value));

            return value;
        }
    }
}