using System;

namespace ShalicoEffect
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AddMenuAttribute : Attribute, IAddMenuAttribute
    {
        public AddMenuAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}