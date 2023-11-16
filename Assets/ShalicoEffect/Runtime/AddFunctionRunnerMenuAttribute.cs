using System;

namespace ShalicoEffect
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AddFunctionRunnerMenuAttribute : AddMenuAttribute
    {
        public AddFunctionRunnerMenuAttribute(string path, int order = 0) : base(path, order)
        {
        }
    }
}