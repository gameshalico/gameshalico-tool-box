using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoAttributePack;
using ShalicoColorPalette;
using ShalicoEffectProcessor.Context;
using ShalicoSoundKit;
using ShalicoToolBox;
using UnityEngine;
using UnityEngine.Audio;

namespace ShalicoEffectProcessor.Effects
{
    [Serializable]
    [CustomDropdownPath("Sound/Play Sound")]
    [CustomListLabel(Tone.Strong)]
    public class PlaySoundEffect : IEffect
    {
        public enum PitchMode
        {
            None,
            EqualTemperament,
            JustIntonation,
            Random,
            RandomByEqualTemperament,
            RandomByJustIntonation
        }

        [SerializeField] private AudioClip audioClip;
        [SerializeField] private AudioMixerGroup audioMixerGroup;

        [SerializeField] private int soundID;
        [SerializeField] private float volume = 1f;

        [Header("Pitch")] [SerializeField] private PitchMode pitchMode;

        [EnableIf(nameof(IsPitchRandom), false)] [SerializeField]
        private float pitch = 1f;

        [ShowIf(nameof(IsPitchRandom), true)] [SerializeField]
        private ValueRange<float> pitchRange = new(-1f, 1f);

        private bool IsPitchRandom => pitchMode == PitchMode.Random ||
                                      pitchMode == PitchMode.RandomByEqualTemperament ||
                                      pitchMode == PitchMode.RandomByJustIntonation;


        public async UniTask PlayEffectAsync(EffectContext context, CancellationToken cancellationToken = default)
        {
            var handler = SoundManager.GetPlayer(audioClip, audioMixerGroup)
                .SetID(soundID)
                .SetVolume(volume);

            SetPitch(handler);

            await handler.PlayAsync(cancellationToken: cancellationToken);
        }

        private void SetPitch(ISoundHandler soundHandler)
        {
            switch (pitchMode)
            {
                case PitchMode.None:
                    soundHandler.SetPitch(pitch);
                    break;
                case PitchMode.EqualTemperament:
                    soundHandler.SetPitchByEqualTemperament(Mathf.RoundToInt(pitch));
                    break;
                case PitchMode.JustIntonation:
                    soundHandler.SetPitchByJustIntonation(Mathf.RoundToInt(pitch));
                    break;
                case PitchMode.Random:
                    soundHandler.SetRandomPitch(pitchRange.Min, pitchRange.Max);
                    break;
                case PitchMode.RandomByEqualTemperament:
                    soundHandler.SetRandomPitchByEqualTemperament(
                        Mathf.RoundToInt(pitchRange.Min),
                        Mathf.RoundToInt(pitchRange.Max)
                    );
                    break;
                case PitchMode.RandomByJustIntonation:
                    soundHandler.SetRandomPitchByJustIntonation(
                        Mathf.RoundToInt(pitchRange.Min),
                        Mathf.RoundToInt(pitchRange.Max)
                    );
                    break;
            }
        }
    }
}