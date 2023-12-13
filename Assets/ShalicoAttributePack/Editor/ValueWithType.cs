using System;
using UnityEngine;

namespace ShalicoAttributePack.Editor
{
    [Serializable]
    public class ValueWithType
    {
        [SerializeField] private string type;
        [SerializeField] private string value;

        private ValueWithType(object value)
        {
            this.value = JsonUtility.ToJson(value);
            type = value.GetType().AssemblyQualifiedName;
        }

        private object GetValue()
        {
            return JsonUtility.FromJson(value, Type.GetType(type));
        }

        public static string ToJson(object value)
        {
            return JsonUtility.ToJson(new ValueWithType(value));
        }

        public static object FromJson(string json)
        {
            return JsonUtility.FromJson<ValueWithType>(json).GetValue();
        }

        public static bool TryParse(string json, out object value)
        {
            try
            {
                value = FromJson(json);
                return true;
            }
            catch (Exception)
            {
                value = null;
                return false;
            }
        }
    }
}