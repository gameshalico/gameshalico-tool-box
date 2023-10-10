using UnityEngine;

namespace ShalicoAttributePack.Runtime
{
    public class BoolNameAttribute : PropertyAttribute
    {
        public string PropertyName;

        public BoolNameAttribute(string propertyName)
        {
            PropertyName = propertyName;
        }
    }
}