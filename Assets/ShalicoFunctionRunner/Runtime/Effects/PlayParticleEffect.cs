using System;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoFunctionRunner.Effects
{
    [Serializable]
    [AddEffectMenu("Particle System/Play")]
    [CustomListLabel("Play Particle System", Tone.Light, HueSymbol.Yellow)]
    public class PlayParticleEffect : ImmediateEffect
    {
        [SerializeField] private ParticleSystem particleSystem;

        protected override void PlayEffectImmediate()
        {
            particleSystem.Play();
        }
    }
}