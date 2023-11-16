using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace ShalicoSoundKit
{
    public interface ISoundHandler
    {
        public bool IsValid { get; }
        public int SoundID { get; }

        public IObservable<Unit> OnReleaseAsObservable { get; }
        public ISoundHandler Play(float volume = 1f);
        public ISoundHandler Stop();
        public void Release();
        public ISoundHandler SetVolume(float volume);
        public ISoundHandler SetPitch(float pitch);
        public ISoundHandler SetLoop(bool loop);
        public ISoundHandler SetID(int soundID);

        public UniTask TweenVolumeUpAsync(float duration, float volume,
            CancellationToken cancellationToken = default);

        public UniTask TweenVolumeDownAsync(float duration, float volume,
            CancellationToken cancellationToken = default);
    }
}