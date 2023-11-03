using System;

namespace ShalicoEffect
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class AddEffectMenuAttribute : Attribute
    {
        public AddEffectMenuAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}