namespace ShalicoDesignPatterns
{
    public interface ICommand
    {
        void Execute();
        void Undo();
    }
}