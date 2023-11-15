using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ShalicoEffect
{
    [Serializable]
    public class EffectGroup
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

        public void RemoveEffectAt(int index)
        {
            var effects = _effects;
            Array.Copy(effects, index + 1, effects, index, effects.Length - index - 1);
            Array.Resize(ref effects, effects.Length - 1);
            _effects = effects;
        }

        public void RemoveEffect(IEffect effect)
        {
            var effects = _effects;
            var index = Array.IndexOf(effects, effect);
            if (index < 0)
                return;
            RemoveEffectAt(index);
        }

        public void ClearEffects()
        {
            _effects = Array.Empty<IEffect>();
        }

        public void Play(CancellationToken token)
        {
            foreach (var effect in _effects)
                effect.PlayEffectAsync(token).Forget();
        }

        public async UniTask PlayAsync(CancellationToken token)
        {
            await UniTask.WhenAll(_effects.Select(effect => effect.PlayEffectAsync(token)));
        }
    }
}