using System;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;

namespace ShalicoEffectProcessor.Effects
{
    [Serializable]
    [CustomDropdownPath("Particle System/Play Particle System")]
    [CustomListLabel(Tone.Strong, HueSymbol.Yellow)]
    public class PlayParticleEffect : ImmediateEffect
    {
        [SerializeField] private ParticleSystem particleSystem;

        protected override void PlayEffectImmediate(EffectContext context)
        {
            particleSystem.Play();
        }
    }
}