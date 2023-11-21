using UnityEngine;

namespace ShalicoColorPalette
{
    public interface IColorData
    {
        public string ColorName { get; }
        public Color32 Color { get; }
    }
}