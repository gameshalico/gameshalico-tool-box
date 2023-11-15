using System;
using ShalicoPalette;
using UnityEngine;

namespace ShalicoEffect
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false)]
    public class CustomListLabelAttribute : Attribute
    {
        private readonly HueSymbol _hueSymbol;

        private readonly Tone _tone;

        public CustomListLabelAttribute(string text, Tone tone, HueSymbol hueSymbol = HueSymbol.Red)
        {
            Text = text;
            _tone = tone;
            _hueSymbol = hueSymbol;
        }

        public Color32 Color => Tones.GetColor(_tone, _hueSymbol);
        public string Text { get; }
    }
}