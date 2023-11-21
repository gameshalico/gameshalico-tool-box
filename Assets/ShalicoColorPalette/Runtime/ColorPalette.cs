using System;
using UnityEngine;

namespace ShalicoColorPalette
{
    [Serializable]
    public class ColorData : IColorData
    {
        [SerializeField] private string colorName;
        [SerializeField] private Color32 color;

        public ColorData(string colorName, Color32 color)
        {
            this.colorName = colorName;
            this.color = color;
        }

        public string ColorName => colorName;
        public Color32 Color => color;
    }

    [CreateAssetMenu(fileName = "ColorPalette", menuName = "ShalicoPalette/Palette/ColorPalette", order = 0)]
    public class ColorPalette : PaletteBase
    {
        [SerializeField] private ColorData[] colors =
        {
            new("Primary", Colors.Red),
            new("Primary Variant", Colors.Magenta),
            new("Secondary", Colors.Blue),
            new("Secondary Variant", Colors.Cyan),
            new("Background", Colors.White),
            new("Surface", Colors.Gray),
            new("Error", Colors.Red),
            new("On Primary", Colors.White),
            new("On Secondary", Colors.White),
            new("On Background", Colors.Black),
            new("On Surface", Colors.Black),
            new("On Error", Colors.White)
        };

        public override bool TryGetColorData(int index, out IColorData colorData)
        {
            if (index < 0 || index >= colors.Length)
            {
                colorData = null;
                return false;
            }

            colorData = colors[index];
            return true;
        }

        public override bool TryFindColorDataByName(string colorName, out IColorData colorData)
        {
            colorData = Array.Find(colors, color => color.ColorName == colorName);
            return colorData != null;
        }

        public override IColorData[] GetAllColorData()
        {
            return colors;
        }
    }
}