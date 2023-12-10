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
    }
}