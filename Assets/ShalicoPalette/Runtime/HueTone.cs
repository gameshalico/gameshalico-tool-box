using System;
using UnityEngine;

namespace ShalicoPalette
{
    [Serializable]
    public struct HueTone
    {
        public HueSymbol hue;
        public Tone tone;

        public HueTone(HueSymbol hue, Tone tone)
        {
            this.hue = hue;
            this.tone = tone;
        }

        public HueTone(int hueIndex, Tone tone)
        {
            hue = (HueSymbol)hueIndex;
            this.tone = tone;
        }

        public HueTone ShiftHue(int shift)
        {
            return new HueTone(hue.Shift(shift), tone);
        }

        public HueTone GetComplementary()
        {
            return new HueTone(hue.Shift(12), tone);
        }

        public Color32 GetColor()
        {
            return Tones.GetColor(tone, hue);
        }
    }
}