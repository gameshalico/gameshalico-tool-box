namespace ShalicoDesignPatterns
{
    public interface IFactory<out TProduct>
    {
        TProduct Create();
    }
}