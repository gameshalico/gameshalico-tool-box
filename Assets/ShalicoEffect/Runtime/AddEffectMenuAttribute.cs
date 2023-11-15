using System;
using ShalicoAttributePack;

namespace ShalicoEffect
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AddEffectMenuAttribute : AddMenuAttribute
    {
        public AddEffectMenuAttribute(string path) : base(path)
        {
        }
    }
}