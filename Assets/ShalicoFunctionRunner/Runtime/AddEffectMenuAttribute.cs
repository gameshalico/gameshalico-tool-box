using System;

namespace ShalicoFunctionRunner
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AddEffectMenuAttribute : AddMenuAttribute
    {
        public AddEffectMenuAttribute(string path, int order = 0) : base(path, order)
        {
        }
    }
}