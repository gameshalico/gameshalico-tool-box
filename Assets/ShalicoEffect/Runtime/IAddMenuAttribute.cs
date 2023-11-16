namespace ShalicoEffect
{
    public interface IAddMenuAttribute
    {
        public string Path { get; }
        public int Order => 0;
    }
}