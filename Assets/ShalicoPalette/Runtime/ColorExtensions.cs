using UnityEngine;

namespace ShalicoPalette
{
    public static class ColorExtensions
    {
        public static Color Alpha(this Color color, float alpha)
        {
            return new Color(color.r, color.g, color.b, alpha);
        }

        public static Color32 Alpha(this Color32 color, byte alpha)
        {
            return new Color32(color.r, color.g, color.b, alpha);
        }

        public static Color ShiftHue(this Color color, float shift)
        {
            Color.RGBToHSV(color, out var hue, out var saturation, out var value);
            hue = (hue * 360 + shift) % 360 / 360.0f;
            return Color.HSVToRGB(hue, saturation, value);
        }

        public static Color ShiftSaturation(this Color color, float shift)
        {
            Color.RGBToHSV(color, out var hue, out var saturation, out var value);
            saturation = Mathf.Clamp01(saturation + shift);
            return Color.HSVToRGB(hue, saturation, value);
        }

        public static Color ShiftValue(this Color color, float shift)
        {
            Color.RGBToHSV(color, out var hue, out var saturation, out var value);
            value = Mathf.Clamp01(value + shift);
            return Color.HSVToRGB(hue, saturation, value);
        }

        public static Color GetComplementary(this Color color)
        {
            return color.ShiftHue(180);
        }
    }
}