using System;
using ShalicoAttributePack;

namespace ShalicoEffect
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AddFunctionRunnerMenuAttribute : AddMenuAttribute
    {
        public AddFunctionRunnerMenuAttribute(string path) : base(path)
        {
        }
    }
}