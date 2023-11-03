using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using ShalicoAttributePack;
using ShalicoSoundKit.Runtime;
using ShalicoToolBox;
using UnityEngine;
using UnityEngine.Audio;

namespace ShalicoEffect.Effects
{
    [Serializable]
    [AddEffectMenu("Sound/Play Sound")]
    public class PlaySoundEffect : Effect
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

        [DisableIf(nameof(IsPitchRandom))] [SerializeField]
        private float pitch = 1f;

        [EnableIf(nameof(IsPitchRandom))] [SerializeField]
        private ValueRange<float> pitchRange = new(-1f, 1f);

        private bool IsPitchRandom => pitchMode == PitchMode.Random ||
                                      pitchMode == PitchMode.RandomByEqualTemperament ||
                                      pitchMode == PitchMode.RandomByJustIntonation;

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
                    soundHandler.SetRandomPitch(pitchRange.min, pitchRange.max);
                    break;
                case PitchMode.RandomByEqualTemperament:
                    soundHandler.SetRandomPitchByEqualTemperament(
                        Mathf.RoundToInt(pitchRange.min),
                        Mathf.RoundToInt(pitchRange.max)
                    );
                    break;
                case PitchMode.RandomByJustIntonation:
                    soundHandler.SetRandomPitchByJustIntonation(
                        Mathf.RoundToInt(pitchRange.min),
                        Mathf.RoundToInt(pitchRange.max)
                    );
                    break;
            }
        }

        protected override async UniTask PlayEffectAsync(CancellationToken cancellationToken)
        {
            var handler = SoundManager.GetPlayer(audioClip, audioMixerGroup)
                .SetID(soundID)
                .SetVolume(volume);

            SetPitch(handler);

            await handler.Play().OnReleaseAsObservable.ToUniTask(cancellationToken: cancellationToken);
        }
    }
}