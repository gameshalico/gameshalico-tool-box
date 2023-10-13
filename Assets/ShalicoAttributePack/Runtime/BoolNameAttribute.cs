using UnityEngine;

namespace ShalicoAttributePack
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