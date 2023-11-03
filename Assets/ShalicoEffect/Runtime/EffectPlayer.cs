using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShalicoEffect
{
    public class EffectPlayer : MonoBehaviour
    {
        [SerializeReference] private IEffect[] _effects = Array.Empty<IEffect>();

        public void AddEffect(IEffect effect)
        {
            var effects = _effects;
            Array.Resize(ref effects, effects.Length + 1);
            effects[^1] = effect;
            _effects = effects;
        }

        public void SetEffectData<T>(T data)
        {
            foreach (var effect in _effects)
                if (effect is IEffectDataReceiver<T> receiver)
                    receiver.SetData(data);
        }

        public void PlayEffects(CancellationToken token)
        {
            foreach (var effect in _effects)
                effect.PlayAsync(token).Forget();
        }
    }
}