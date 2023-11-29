using System;
using UnityEngine;

namespace ShalicoEffectProcessor.Editor
{
    public static class EffectUtility
    {
        public static string SerializeEffect(IEffect effect)
        {
            var serializableEffect = new SerializableEffect(effect);
            var json = JsonUtility.ToJson(serializableEffect);
            return json;
        }

        public static bool TryDeserializeEffect(string json, out IEffect effect)
        {
            effect = null;
            if (string.IsNullOrEmpty(json))
                return false;

            try
            {
                var serializableEffect = JsonUtility.FromJson<SerializableEffect>(json);
                effect = serializableEffect.GetEffect();
            }
            catch (ArgumentException)
            {
                return false;
            }

            return true;
        }

        [Serializable]
        public class SerializableEffect
        {
            public string json;
            public string typeName;

            public SerializableEffect(IEffect effect)
            {
                json = JsonUtility.ToJson(effect);
                typeName = effect.GetType().AssemblyQualifiedName;
            }

            public IEffect GetEffect()
            {
                var type = Type.GetType(typeName);
                return JsonUtility.FromJson(json, type) as IEffect;
            }
        }
    }
}