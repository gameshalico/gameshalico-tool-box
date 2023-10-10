using System.IO;
using UnityEngine;

namespace ShalicoSoundKit.Runtime
{
    public static class WavUtility
    {
        public static void SaveFromClip(string filepath, AudioClip clip)
        {
            var fileStream = new FileStream(filepath, FileMode.Create);
            var binaryWriter = new BinaryWriter(fileStream);

            binaryWriter.Write(new char[4] { 'R', 'I', 'F', 'F' });
            binaryWriter.Write(36 + clip.samples * clip.channels); // File size - 8
            binaryWriter.Write(new char[4] { 'W', 'A', 'V', 'E' });
            binaryWriter.Write(new char[4] { 'f', 'm', 't', ' ' });

            binaryWriter.Write(16); // Size of the following chunk
            binaryWriter.Write((ushort)1); // Audio format (1 = PCM)
            binaryWriter.Write((ushort)clip.channels);
            binaryWriter.Write(clip.frequency);
            binaryWriter.Write(clip.frequency * clip.channels * 2); // Bytes per second
            binaryWriter.Write((ushort)(clip.channels * 2)); // Bytes per sample * channels
            binaryWriter.Write((ushort)16); // Bits per sample

            binaryWriter.Write(new char[4] { 'd', 'a', 't', 'a' });
            binaryWriter.Write(clip.samples * clip.channels * 2); // Size of the following chunk

            var samples = new float[clip.samples * clip.channels];
            clip.GetData(samples, 0);
            foreach (var sample in samples) binaryWriter.Write((short)(sample * short.MaxValue));

            binaryWriter.Close();
            fileStream.Close();
        }
    }
}