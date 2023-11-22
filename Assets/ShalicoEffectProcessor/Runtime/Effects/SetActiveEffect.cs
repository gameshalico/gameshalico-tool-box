using System;
using ShalicoColorPalette;
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

        protected override void PlayEffectImmediate()
        {
            target.SetActive(active);
        }
    }
}