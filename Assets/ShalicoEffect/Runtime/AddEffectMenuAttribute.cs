using System;
using ShalicoAttributePack;

namespace ShalicoEffect
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AddEffectMenuAttribute : AddMenuAttribute
    {
        public AddEffectMenuAttribute(string path, int order = 0) : base(path, order)
        {
        }
    }
}