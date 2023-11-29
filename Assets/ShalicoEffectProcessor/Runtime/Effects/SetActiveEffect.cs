using System;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;

namespace ShalicoEffectProcessor.Effects
{
    [Serializable]
    [AddEffectMenu("Game Object/Set Active")]
    [CustomListLabel("Set Active", Tone.Strong, HueSymbol.Blue2)]
    public class SetActiveEffect : ImmediateEffect
    {
        [SerializeField] private GameObject target;
        [SerializeField] private bool active;

        protected override void PlayEffectImmediate(EffectContext context)
        {
            target.SetActive(active);
        }
    }
}