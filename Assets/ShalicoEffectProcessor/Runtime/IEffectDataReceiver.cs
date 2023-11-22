namespace ShalicoEffectProcessor
{
    public interface IEffectDataReceiver<T>
    {
        public void SetData(T data);
    }
}