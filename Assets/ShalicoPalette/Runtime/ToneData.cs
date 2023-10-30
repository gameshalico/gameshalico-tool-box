using UnityEngine;

namespace ShalicoPalette
{
    public class ToneData
    {
        private readonly Color32[] _colors;
        private readonly bool _isNeutral;
        private readonly bool _isVivid;

        public ToneData(Color32[] colors, ColorType type, SaturationLevel level, bool isVivid = false,
            bool isNeutral = false)
        {
            _colors = colors;
            _isVivid = isVivid;
            ColorType = type;
            SaturationLevel = level;
            _isNeutral = isNeutral;
        }

        public Color32 this[int index]
        {
            get
            {
                if (_isNeutral)
                    index = 0;
                else if (!_isVivid)
                    index /= 2;
                return _colors[index];
            }
        }

        public Color this[HueSymbol hue] => this[(int)hue - 1];

        public ColorType ColorType { get; }

        public SaturationLevel SaturationLevel { get; }
    }
}