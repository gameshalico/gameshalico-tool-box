using System;
using System.Linq;

namespace ShalicoAttributePack
{
    [AttributeUsage(AttributeTargets.Class)]
    public class CustomDropdownPathAttribute : Attribute
    {
        public CustomDropdownPathAttribute(string path = "")
        {
            Path = path;
        }

        public string Path { get; }
        public string Name => Path.Split('/').Last();
    }
}