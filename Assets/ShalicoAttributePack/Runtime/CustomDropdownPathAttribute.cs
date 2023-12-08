using System;

namespace ShalicoAttributePack
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomDropdownPathAttribute : Attribute
    {
        public CustomDropdownPathAttribute(string path = "", string name = "")
        {
            Path = path;
            Name = name;
        }

        public string Path { get; }
        public string Name { get; }
    }
}