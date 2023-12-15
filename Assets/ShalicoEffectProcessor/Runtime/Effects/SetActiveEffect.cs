using System;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;

namespace ShalicoEffectProcessor.Effects
{
    [Serializable]
    [CustomDropdownPath("Game Object/Set Active")]
    [CustomListLabel(Tone.Strong, HueSymbol.Blue2)]
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