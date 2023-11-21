using System;
using UnityEngine;

namespace ShalicoColorPalette
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

        public Color32 ToColor()
        {
            return Tones.GetColor(tone, hue);
        }

        public static implicit operator Color32(HueTone hueTone)
        {
            return hueTone.ToColor();
        }

        public static implicit operator Color(HueTone hueTone)
        {
            return hueTone.ToColor();
        }

        public override string ToString()
        {
            var toneData = Tones.GetTone(tone);

            if (toneData.IsVivid)
                return $"{toneData.ToneSymbol} {(int)hue}";
            if (toneData.IsNeutral)
                return $"{toneData.ToneSymbol}";
            return $"{toneData.ToneSymbol}{Mathf.Ceil((float)hue / 2) * 2}";
        }
    }
}