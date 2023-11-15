using System;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.Effects
{
    [Serializable]
    [AddEffectMenu("Game Object/Set Active")]
    [CustomListLabel("Set Active", Tone.Light, HueSymbol.Blue2)]
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