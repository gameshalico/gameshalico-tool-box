using System;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.Effects
{
    [Serializable]
    [AddEffectMenu("Debug/Debug Log")]
    [EffectCustomHeader("Debug Log", Tone.MediumGray1, HueSymbol.Red)]
    public class DebugLogEffect : ImmediateEffect
    {
        [SerializeField] private HueTone color;
        [SerializeField] private string message;

        public DebugLogEffect()
        {
            color = new HueTone(HueSymbol.Red, Tone.Pale);
        }

        protected override void PlayEffectImmediate()
        {
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color.ToColor())}>{message}</color>");
        }
    }
}