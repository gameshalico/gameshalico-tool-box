using System;
using UnityEngine;

namespace ShalicoColorPalette
{
    [Serializable]
    public class HueToneData : IColorData
    {
        [SerializeField] private string colorName;
        [SerializeField] private HueTone hueTone;

        public HueToneData(string colorName, HueTone hueTone)
        {
            this.colorName = colorName;
            this.hueTone = hueTone;
        }

        public string ColorName => colorName;
        public Color32 Color => hueTone.ToColor();
    }

    [CreateAssetMenu(fileName = "HueTonePalette", menuName = "ShalicoPalette/Palette/HueTonePalette", order = 0)]
    public class HueTonePalette : PaletteBase
    {
        [SerializeField] private HueToneData[] colors =
        {
            new("Primary", new HueTone(HueSymbol.Red, Tone.Vivid)),
            new("Primary Variant", new HueTone(HueSymbol.Red.Shift(2), Tone.Vivid)),
            new("Secondary", new HueTone(HueSymbol.Blue2, Tone.Vivid)),
            new("Secondary Variant", new HueTone(HueSymbol.Blue2.Shift(2), Tone.Vivid)),
            new("Background", new HueTone(HueSymbol.Red, Tone.White)),
            new("Surface", new HueTone(HueSymbol.Red, Tone.White)),
            new("Error", new HueTone(HueSymbol.RedPurple, Tone.Vivid)),
            new("On Primary", new HueTone(HueSymbol.Red, Tone.White)),
            new("On Secondary", new HueTone(HueSymbol.Red, Tone.White)),
            new("On Background", new HueTone(HueSymbol.Red, Tone.DarkGray1)),
            new("On Surface", new HueTone(HueSymbol.Red, Tone.DarkGray1)),
            new("On Error", new HueTone(HueSymbol.Red, Tone.White))
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