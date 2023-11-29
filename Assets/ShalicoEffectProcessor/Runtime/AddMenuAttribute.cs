using System;

namespace ShalicoEffectProcessor
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AddMenuAttribute : Attribute, IAddMenuAttribute
    {
        protected AddMenuAttribute(string path, int order = 0)
        {
            Path = path;
            Order = order;
        }

        public string Path { get; }
        public int Order { get; }
    }
}