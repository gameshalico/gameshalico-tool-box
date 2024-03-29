using System.Collections.Generic;

namespace ShalicoDesignPatterns
{
    public class CommandInvoker
    {
        private readonly Stack<ICommand> _undoStack = new();

        public void Execute(ICommand command)
        {
            command.Execute();
            _undoStack.Push(command);
        }

        public void Undo()
        {
            if (_undoStack.Count == 0)
                return;

            var command = _undoStack.Pop();
            command.Undo();
        }
    }
}