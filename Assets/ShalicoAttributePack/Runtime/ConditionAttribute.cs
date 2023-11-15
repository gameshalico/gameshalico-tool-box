using UnityEngine;

namespace ShalicoAttributePack
{
    public class ConditionAttribute : PropertyAttribute
    {
        public readonly string ConditionName;
        public readonly object Value;

        protected ConditionAttribute(string conditionName, object value)
        {
            ConditionName = conditionName;
            Value = value;
        }
    }
}