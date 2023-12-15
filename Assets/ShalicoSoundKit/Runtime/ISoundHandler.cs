using System.Threading;
using Cysharp.Threading.Tasks;

namespace ShalicoSoundKit
{
    public interface ISoundHandler
    {
        public bool IsValid { get; }
        public int SoundGroupID { get; }

        public ISoundHandler Play();
        public UniTask PlayAsync(CancellationToken cancellationToken = default);
        public ISoundHandler Stop();
        public void Release();
        public ISoundHandler SetVolume(float volume);
        public ISoundHandler SetPitch(float pitch);
        public ISoundHandler SetLoop(bool loop);
        public ISoundHandler SetGroupID(int soundGroupID);

        public UniTask TweenVolumeUpAsync(float duration, float volume,
            CancellationToken cancellationToken = default);

        public UniTask TweenVolumeDownAsync(float duration, float volume,
            CancellationToken cancellationToken = default);
    }
}