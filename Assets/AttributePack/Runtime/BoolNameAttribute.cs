using UnityEngine;

namespace AttributePack
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