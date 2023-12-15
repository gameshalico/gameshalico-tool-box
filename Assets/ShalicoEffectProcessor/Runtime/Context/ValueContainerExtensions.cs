namespace ShalicoEffectProcessor.Context
{
    public static class ValueContainerExtensions
    {
        public static ValueContainer<T> GetContainer<T>(this EffectContext context)
        {
            if (context.TryGet(out ValueContainer<T> container))
                return container;

            var newContainer = ValueContainer<T>.Get(default);
            context.Set(newContainer);
            return newContainer;
        }

        public static bool TryGetContainer<T>(this EffectContext context, out ValueContainer<T> container)
        {
            if (context.TryGet(out container))
                return true;

            return false;
        }


        public static T GetValue<T>(this EffectContext context)
        {
            return context.GetContainer<T>().Value;
        }


        public static bool TryGetValue<T>(this EffectContext context, out T value)
        {
            if (context.TryGetContainer(out ValueContainer<T> container))
            {
                value = container.Value;
                return true;
            }

            value = default;
            return false;
        }
    }
}