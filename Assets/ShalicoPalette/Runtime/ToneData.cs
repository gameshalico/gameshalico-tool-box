using UnityEngine;

namespace ShalicoPalette
{
    public class ToneData
    {
        private readonly Color32[] _colors;

        public ToneData(string toneSymbol, Color32[] colors, ColorType type, SaturationLevel level,
            bool isVivid = false,
            bool isNeutral = false)
        {
            ToneSymbol = toneSymbol;
            _colors = colors;
            IsVivid = isVivid;
            ColorType = type;
            SaturationLevel = level;
            IsNeutral = isNeutral;
        }

        public Color32 this[int index]
        {
            get
            {
                if (index < 0)
                    return Colors.Black;
                if (IsNeutral)
                    index = 0;
                else if (!IsVivid)
                    index /= 2;
                return _colors[index];
            }
        }

        public Color this[HueSymbol hue] => this[(int)hue - 1];

        public ColorType ColorType { get; }

        public SaturationLevel SaturationLevel { get; }

        public string ToneSymbol { get; }

        public bool IsNeutral { get; }

        public bool IsVivid { get; }
    }
}