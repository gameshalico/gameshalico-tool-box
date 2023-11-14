using System;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class EffectCustomHeaderAttribute : Attribute
    {
        private readonly HueSymbol _hueSymbol;

        private readonly Tone _tone;

        public EffectCustomHeaderAttribute(string text, Tone tone, HueSymbol hueSymbol)
        {
            Text = text;
            _tone = tone;
            _hueSymbol = hueSymbol;
        }

        public Color32 Color => Tones.GetColor(_tone, _hueSymbol);
        public string Text { get; }
    }
}