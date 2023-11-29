using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoColorPalette;
using UnityEngine;

namespace ShalicoEffectProcessor.Effects
{
    [Serializable]
    [CustomListLabel("Group", Tone.Strong, HueSymbol.RedPurple)]
    public class EffectGroup : IEffect
    {
        [SerializeReference] private IEffect[] _effects = Array.Empty<IEffect>();

        public async UniTask PlayEffectAsync(EffectContext context, CancellationToken cancellationToken = default)
        {
            await UniTask.WhenAll(_effects.Select(effect => effect.PlayEffectAsync(context, cancellationToken)));
        }

        public void AddEffect(IEffect effect)
        {
            var effects = _effects;
            Array.Resize(ref effects, effects.Length + 1);
            effects[^1] = effect;
            _effects = effects;
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

        public void Play(EffectContext context, CancellationToken cancellationToken)
        {
            foreach (var effect in _effects)
                effect.PlayEffectAsync(context, cancellationToken).Forget();
        }
    }
}