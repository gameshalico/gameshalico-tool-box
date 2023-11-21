using UnityEngine;

namespace ShalicoColorPalette
{
    public static class ColorResolver
    {
        private static IPalette s_palette;

        static ColorResolver()
        {
            s_palette = PaletteSettings.DefaultPalette;
        }

        public static Color32 Resolve(IndexedColor indexedColor)
        {
            if (s_palette == null) s_palette = PaletteSettings.DefaultPalette;
            if (s_palette == null)
            {
                Debug.LogWarning("[Shalico Palette] Please set default palette");
                return Colors.Magenta;
            }

            if (s_palette.TryGetColorData(indexedColor.colorIndex, out var colorData)) return colorData.Color;
            Debug.LogWarning($"[Shalico Palette] Color with index {indexedColor.colorIndex} not found");
            return Colors.Magenta;
        }

        public static Color32 Resolve(string colorName)
        {
            if (s_palette == null) s_palette = PaletteSettings.DefaultPalette;
            if (s_palette == null)
            {
                Debug.LogWarning("[Shalico Palette] Please set default palette");
                return Colors.Magenta;
            }

            if (s_palette.TryGetColorData(int.Parse(colorName), out var colorData)) return colorData.Color;

            Debug.LogWarning($"[Shalico Palette] Color with index {colorName} not found");
            return Colors.Magenta;
        }

        public static void SetPalette(IPalette palette)
        {
            s_palette = palette;
        }
    }
}