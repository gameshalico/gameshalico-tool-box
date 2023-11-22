using System;

namespace ShalicoEffectProcessor
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AddEffectProcessorMenuAttribute : AddMenuAttribute
    {
        public AddEffectProcessorMenuAttribute(string path, int order = 0) : base(path, order)
        {
        }
    }
}