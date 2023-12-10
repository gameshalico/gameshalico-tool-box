namespace ShalicoEffectProcessor.Context
{
    public static class ValueContainerExtensions
    {
        public static ValueContainer<T> GetContainer<T>(this EffectContext context) where T : struct
        {
            if (context.TryGet(out ValueContainer<T> container))
                return container;

            var newContainer = new ValueContainer<T>(default(T));
            context.Set(newContainer);
            return newContainer;
        }
    }
}