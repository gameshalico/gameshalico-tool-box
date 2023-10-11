using System.Linq;
using UnityEngine;

namespace ShalicoSoundKit.Runtime
{
    public static class SoundProcessor
    {
        public static AudioClip NormalizeAmplitude(AudioClip clip, float maxAmplitude = 1f)
        {
            var samples = clip.samples;
            var channels = clip.channels;
            var data = new float[samples * channels];

            clip.GetData(data, 0);

            var currentMaxAmplitude = Mathf.Max(Mathf.Abs(data.Min()), Mathf.Abs(data.Max()));
            var rate = maxAmplitude / currentMaxAmplitude;

            for (var i = 0; i < data.Length; i++) data[i] *= rate;

            var normalizedClip = AudioClip.Create(clip.name, samples, channels, clip.frequency, false);
            normalizedClip.SetData(data, 0);
            return normalizedClip;
        }

        public static AudioClip Trim(AudioClip clip, int startSample, int endSample)
        {
            var samples = endSample - startSample;
            var channels = clip.channels;
            var data = new float[samples * channels];

            clip.GetData(data, startSample);

            var cutoffClip = AudioClip.Create(clip.name, samples, channels, clip.frequency, false);
            cutoffClip.SetData(data, 0);
            return cutoffClip;
        }

        public static int FindBeginningSampleIndex(AudioClip clip, float threshold = 0.0001f)
        {
            var samples = clip.samples;
            var channels = clip.channels;
            var data = new float[samples * channels];

            clip.GetData(data, 0);

            var startSample = 0;
            for (var i = 0; i < data.Length; i++)
                if (Mathf.Abs(data[i]) > threshold)
                {
                    startSample = i;
                    break;
                }

            return startSample;
        }

        public static int FindEndingSampleIndex(AudioClip clip, float threshold = 0.0001f)
        {
            var samples = clip.samples;
            var channels = clip.channels;
            var data = new float[samples * channels];

            clip.GetData(data, 0);

            var endSample = data.Length - 1;
            for (var i = data.Length - 1; i >= 0; i--)
                if (Mathf.Abs(data[i]) > threshold)
                {
                    endSample = i;
                    break;
                }

            return endSample;
        }

        public static AudioClip TrimBeginningSilence(AudioClip clip, float threshold = 0.0001f)
        {
            var samples = clip.samples;
            var channels = clip.channels;
            var data = new float[samples * channels];

            clip.GetData(data, 0);

            var startSample = FindBeginningSampleIndex(clip, threshold);

            return Trim(clip, startSample, samples);
        }

        public static AudioClip TrimEndingSilence(AudioClip clip, float threshold = 0.0001f)
        {
            var samples = clip.samples;
            var channels = clip.channels;
            var data = new float[samples * channels];

            clip.GetData(data, 0);

            var endSample = FindEndingSampleIndex(clip, threshold);

            return Trim(clip, 0, endSample);
        }

        public static AudioClip TrimSilence(AudioClip clip, float threshold = 0.0001f)
        {
            var samples = clip.samples;
            var channels = clip.channels;
            var data = new float[samples * channels];

            clip.GetData(data, 0);

            var startSample = FindBeginningSampleIndex(clip, threshold);
            var endSample = FindEndingSampleIndex(clip, threshold);

            return Trim(clip, startSample, endSample);
        }
    }
}