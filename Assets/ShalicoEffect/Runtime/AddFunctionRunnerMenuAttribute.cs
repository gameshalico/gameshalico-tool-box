using System;

namespace ShalicoEffect
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AddFunctionRunnerMenuAttribute : Attribute, IAddMenuAttribute
    {
        public AddFunctionRunnerMenuAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}