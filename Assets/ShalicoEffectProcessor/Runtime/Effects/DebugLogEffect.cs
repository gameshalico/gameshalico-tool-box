using System;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using UnityEngine;

namespace ShalicoEffectProcessor.Effects
{
    [Serializable]
    [CustomDropdownPath("Debug/Debug Log")]
    [CustomListLabel("Debug Log", Tone.Strong, HueSymbol.RedPurple)]
    public class DebugLogEffect : ImmediateEffect
    {
        [SerializeField] private string message;

        protected override void PlayEffectImmediate(EffectContext context)
        {
            Debug.Log(message);
        }
    }
}