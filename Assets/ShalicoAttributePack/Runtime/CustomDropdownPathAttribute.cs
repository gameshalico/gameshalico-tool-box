using System;

namespace ShalicoAttributePack
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomDropdownPathAttribute : Attribute
    {

        public CustomDropdownPathAttribute(string path)
        {
            Path = path;
        }

        public string Path { get; }
    }
}