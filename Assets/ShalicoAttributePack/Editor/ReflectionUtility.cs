using System.Reflection;

namespace ShalicoAttributePack.Editor
{
    public static class ReflectionUtility
    {
        private const BindingFlags FindAllBindingFlags =
            BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public |
            BindingFlags.Static;

        public static bool TryFindFieldOrPropertyValue<TValue>(object target, string name, out TValue value,
            BindingFlags bindingFlags = FindAllBindingFlags)
        {
            var targetPropertyInfo = target.GetType()
                .GetProperty(name, bindingFlags);
            if (targetPropertyInfo != null && targetPropertyInfo.GetValue(target) is TValue propertyValue)
            {
                value = propertyValue;
                return true;
            }

            var targetFieldInfo = target.GetType()
                .GetField(name, bindingFlags);
            if (targetFieldInfo != null && targetFieldInfo.GetValue(target) is TValue fieldValue)
            {
                value = fieldValue;
                return true;
            }

            value = default;
            return false;
        }
    }
}