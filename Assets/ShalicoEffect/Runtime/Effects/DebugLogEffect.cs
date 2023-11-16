using System;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect.Effects
{
    [Serializable]
    [AddEffectMenu("Debug/Debug Log", -1)]
    [CustomListLabel("Debug Log", Tone.Light, HueSymbol.RedPurple)]
    public class DebugLogEffect : ImmediateEffect
    {
        [SerializeField] private HueTone color;
        [SerializeField] private string message;

        public DebugLogEffect()
        {
            color = new HueTone(HueSymbol.Red, Tone.White);
        }

        protected override void PlayEffectImmediate()
        {
            Debug.Log($"<color=#{ColorUtility.ToHtmlStringRGB(color.ToColor())}>{message}</color>");
        }
    }
}