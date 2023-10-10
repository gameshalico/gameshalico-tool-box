using UnityEngine;

namespace ShalicoSoundKit.Runtime
{
    public static class DecibelUtility
    {
        public static float LinearToDecibel(float linear)
        {
            var dB = Mathf.Clamp(20f * Mathf.Log10(linear), -80f, 0f);
            return dB;
        }

        public static float DecibelToLinear(float dB)
        {
            var linear = Mathf.Pow(10f, dB / 20f);
            return linear;
        }
    }
}