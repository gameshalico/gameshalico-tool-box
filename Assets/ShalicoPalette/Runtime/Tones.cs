using UnityEngine;

namespace ShalicoPalette
{
    public static class Tones
    {
        private static readonly ToneData[] ToneArray =
        {
            new(new Color32[] // Vivid
            {
                new(0xD4, 0x00, 0x45, 0xFF),
                new(0xEE, 0x00, 0x26, 0xFF),
                new(0xFD, 0x1A, 0x1C, 0xFF),
                new(0xFE, 0x41, 0x18, 0xFF),
                new(0xFF, 0x59, 0x0B, 0xFF),
                new(0xFF, 0x7F, 0x00, 0xFF),
                new(0xFF, 0xCC, 0x00, 0xFF),
                new(0xFF, 0xE6, 0x00, 0xFF),
                new(0xCC, 0xE7, 0x00, 0xFF),
                new(0x99, 0xCF, 0x15, 0xFF),
                new(0x66, 0xB8, 0x2B, 0xFF),
                new(0x33, 0xA2, 0x3D, 0xFF),
                new(0x00, 0x8F, 0x62, 0xFF),
                new(0x00, 0x86, 0x78, 0xFF),
                new(0x00, 0x7A, 0x87, 0xFF),
                new(0x05, 0x5D, 0x87, 0xFF),
                new(0x09, 0x3F, 0x86, 0xFF),
                new(0x0F, 0x21, 0x8B, 0xFF),
                new(0x1D, 0x1A, 0x88, 0xFF),
                new(0x28, 0x12, 0x85, 0xFF),
                new(0x34, 0x0C, 0x81, 0xFF),
                new(0x56, 0x00, 0x7D, 0xFF),
                new(0x77, 0x00, 0x71, 0xFF),
                new(0xAF, 0x00, 0x65, 0xFF)
            }, ColorType.Pure, SaturationLevel.High, true),
            new(new Color32[] // Bright
            {
                new(0xF9, 0x34, 0x4C, 0xFF),
                new(0xFC, 0x4E, 0x32, 0xFF),
                new(0xFF, 0x99, 0x14, 0xFF),
                new(0xFF, 0xF2, 0x31, 0xFF),
                new(0x99, 0xD0, 0x2B, 0xFF),
                new(0x33, 0xA6, 0x5E, 0xFF),
                new(0x1A, 0xA1, 0x8E, 0xFF),
                new(0x1D, 0x86, 0xAE, 0xFF),
                new(0x38, 0x6C, 0xB0, 0xFF),
                new(0x69, 0x64, 0xAD, 0xFF),
                new(0xA4, 0x5A, 0xAA, 0xFF),
                new(0xDF, 0x4C, 0x94, 0xFF)
            }, ColorType.LightClear, SaturationLevel.High),
            new(new Color32[] // Strong
            {
                new(0xCA, 0x10, 0x28, 0xFF),
                new(0xCC, 0x46, 0x13, 0xFF),
                new(0xD9, 0x76, 0x0F, 0xFF),
                new(0xCC, 0xB9, 0x14, 0xFF),
                new(0x8C, 0xA1, 0x13, 0xFF),
                new(0x27, 0x85, 0x3F, 0xFF),
                new(0x29, 0x73, 0x64, 0xFF),
                new(0x20, 0x5B, 0x85, 0xFF),
                new(0x23, 0x3B, 0x8B, 0xFF),
                new(0x3D, 0x1C, 0x83, 0xFF),
                new(0x5E, 0x28, 0x83, 0xFF),
                new(0x99, 0x0F, 0x4F, 0xFF)
            }, ColorType.Intermediate, SaturationLevel.High),
            new(new Color32[] // Deep
            {
                new(0x9E, 0x00, 0x2C, 0xFF),
                new(0xA4, 0x12, 0x00, 0xFF),
                new(0xA3, 0x4A, 0x02, 0xFF),
                new(0xA3, 0x82, 0x04, 0xFF),
                new(0x51, 0x85, 0x17, 0xFF),
                new(0x2F, 0x6F, 0x41, 0xFF),
                new(0x02, 0x59, 0x65, 0xFF),
                new(0x04, 0x43, 0x6D, 0xFF),
                new(0x07, 0x3E, 0x73, 0xFF),
                new(0x23, 0x21, 0x66, 0xFF),
                new(0x53, 0x14, 0x60, 0xFF),
                new(0x74, 0x00, 0x50, 0xFF)
            }, ColorType.DarkClear, SaturationLevel.High),
            new(new Color32[] // Light
            {
                new(0xFA, 0x74, 0x82, 0xFF),
                new(0xFB, 0x80, 0x72, 0xFF),
                new(0xFD, 0xB4, 0x6C, 0xFF),
                new(0xFF, 0xF2, 0x7B, 0xFF),
                new(0xB3, 0xDE, 0x69, 0xFF),
                new(0x7F, 0xC9, 0x7E, 0xFF),
                new(0x66, 0xC2, 0xAE, 0xFF),
                new(0x67, 0xB2, 0xCA, 0xFF),
                new(0x67, 0x9F, 0xCA, 0xFF),
                new(0x80, 0x7D, 0xBA, 0xFF),
                new(0xB1, 0x72, 0xB6, 0xFF),
                new(0xE1, 0x70, 0xA4, 0xFF)
            }, ColorType.LightClear, SaturationLevel.Medium),
            new(new Color32[] // Soft
            {
                new(0xC9, 0x5F, 0x6A, 0xFF),
                new(0xD7, 0x78, 0x56, 0xFF),
                new(0xD8, 0x90, 0x48, 0xFF),
                new(0xCC, 0xBA, 0x48, 0xFF),
                new(0xB3, 0xB1, 0x40, 0xFF),
                new(0x66, 0xAC, 0x78, 0xFF),
                new(0x4E, 0x9B, 0x86, 0xFF),
                new(0x4F, 0x8B, 0x97, 0xFF),
                new(0x51, 0x66, 0x90, 0xFF),
                new(0x5D, 0x57, 0x91, 0xFF),
                new(0x8C, 0x55, 0x88, 0xFF),
                new(0xB1, 0x50, 0x76, 0xFF)
            }, ColorType.Intermediate, SaturationLevel.Medium),
            new(new Color32[] // Dull
            {
                new(0x99, 0x41, 0x52, 0xFF),
                new(0xB2, 0x44, 0x43, 0xFF),
                new(0xB2, 0x59, 0x38, 0xFF),
                new(0x99, 0x7F, 0x42, 0xFF),
                new(0x74, 0x7E, 0x47, 0xFF),
                new(0x5A, 0x80, 0x4B, 0xFF),
                new(0x2A, 0x6B, 0x68, 0xFF),
                new(0x1E, 0x62, 0x82, 0xFF),
                new(0x21, 0x42, 0x74, 0xFF),
                new(0x3A, 0x36, 0x7B, 0xFF),
                new(0x5E, 0x31, 0x79, 0xFF),
                new(0x80, 0x2A, 0x68, 0xFF)
            }, ColorType.Intermediate, SaturationLevel.Medium),
            new(new Color32[] // Dark
            {
                new(0x63, 0x2A, 0x31, 0xFF),
                new(0x74, 0x35, 0x26, 0xFF),
                new(0x6C, 0x49, 0x19, 0xFF),
                new(0x69, 0x5B, 0x18, 0xFF),
                new(0x56, 0x56, 0x1A, 0xFF),
                new(0x34, 0x59, 0x34, 0xFF),
                new(0x1D, 0x4B, 0x44, 0xFF),
                new(0x0E, 0x42, 0x51, 0xFF),
                new(0x16, 0x34, 0x4F, 0xFF),
                new(0x31, 0x2C, 0x4B, 0xFF),
                new(0x4A, 0x30, 0x4B, 0xFF),
                new(0x63, 0x31, 0x42, 0xFF)
            }, ColorType.Intermediate, SaturationLevel.Medium),
            new(new Color32[] // Pale
            {
                new(0xFB, 0xB4, 0xC4, 0xFF),
                new(0xFD, 0xCD, 0xB7, 0xFF),
                new(0xFE, 0xE6, 0xAA, 0xFF),
                new(0xFF, 0xFF, 0xB3, 0xFF),
                new(0xE6, 0xF5, 0xB0, 0xFF),
                new(0xCC, 0xEB, 0xC5, 0xFF),
                new(0xB3, 0xE2, 0xD8, 0xFF),
                new(0xB3, 0xD7, 0xDD, 0xFF),
                new(0xB3, 0xCD, 0xE3, 0xFF),
                new(0xB2, 0xB6, 0xD8, 0xFF),
                new(0xCA, 0xB2, 0xD6, 0xFF),
                new(0xE3, 0xAD, 0xD5, 0xFF)
            }, ColorType.LightClear, SaturationLevel.Low),
            new(new Color32[] // LightGrayish
            {
                new(0xD7, 0xA4, 0xB6, 0xFF),
                new(0xD8, 0xAF, 0xA7, 0xFF),
                new(0xD9, 0xBA, 0x97, 0xFF),
                new(0xD9, 0xC6, 0x9B, 0xFF),
                new(0xAA, 0xC0, 0x9B, 0xFF),
                new(0x9E, 0xBC, 0xA3, 0xFF),
                new(0x91, 0xB8, 0xAC, 0xFF),
                new(0x91, 0xAF, 0xBB, 0xFF),
                new(0x91, 0xA4, 0xB6, 0xFF),
                new(0x91, 0x91, 0xAD, 0xFF),
                new(0xA9, 0x97, 0xB0, 0xFF),
                new(0xC0, 0x9E, 0xB3, 0xFF)
            }, ColorType.Intermediate, SaturationLevel.Low),
            new(new Color32[] // Grayish
            {
                new(0x7D, 0x4F, 0x5B, 0xFF),
                new(0x7D, 0x5F, 0x61, 0xFF),
                new(0x7D, 0x67, 0x64, 0xFF),
                new(0x7D, 0x6F, 0x59, 0xFF),
                new(0x63, 0x6E, 0x5B, 0xFF),
                new(0x48, 0x6C, 0x5C, 0xFF),
                new(0x38, 0x5B, 0x63, 0xFF),
                new(0x38, 0x4E, 0x5D, 0xFF),
                new(0x38, 0x41, 0x58, 0xFF),
                new(0x3F, 0x2F, 0x50, 0xFF),
                new(0x49, 0x37, 0x53, 0xFF),
                new(0x5A, 0x3A, 0x54, 0xFF)
            }, ColorType.Intermediate, SaturationLevel.Low),
            new(new Color32[] // DarkGrayish
            {
                new(0x3A, 0x2C, 0x2E, 0xFF),
                new(0x3A, 0x2C, 0x2A, 0xFF),
                new(0x46, 0x3B, 0x34, 0xFF),
                new(0x46, 0x40, 0x2C, 0xFF),
                new(0x3E, 0x3F, 0x31, 0xFF),
                new(0x24, 0x34, 0x2D, 0xFF),
                new(0x25, 0x35, 0x32, 0xFF),
                new(0x28, 0x35, 0x39, 0xFF),
                new(0x22, 0x28, 0x31, 0xFF),
                new(0x28, 0x24, 0x30, 0xFF),
                new(0x2E, 0x2A, 0x31, 0xFF),
                new(0x3A, 0x2D, 0x31, 0xFF)
            }, ColorType.DarkClear, SaturationLevel.Low),
            new(new Color32[] // White
            {
                new(0xFF, 0xFF, 0xFF, 0xFF)
            }, ColorType.Neutral, SaturationLevel.Neutral, isNeutral: true),
            new(new Color32[] // LightGray
            {
                new(0xC0, 0xC0, 0xC0, 0xFF)
            }, ColorType.Neutral, SaturationLevel.Neutral, isNeutral: true),
            new(new Color32[] // MediumGray
            {
                new(0x80, 0x80, 0x80, 0xFF)
            }, ColorType.Neutral, SaturationLevel.Neutral, isNeutral: true),

            new(new Color32[] // DarkGray
            {
                new(0x40, 0x40, 0x40, 0xFF)
            }, ColorType.Neutral, SaturationLevel.Neutral, isNeutral: true),

            new(new Color32[] // Black
            {
                new(0x00, 0x00, 0x00, 0xFF)
            }, ColorType.Neutral, SaturationLevel.Neutral, isNeutral: true)
        };

        public static ToneData GetTone(Tone tone)
        {
            return ToneArray[(int)tone];
        }

        public static Color32 GetColor(Tone tone, int hue)
        {
            return ToneArray[(int)tone][hue];
        }

        public static Color32 GetColor(Tone tone, HueSymbol hue)
        {
            return ToneArray[(int)tone][hue];
        }
    }
}