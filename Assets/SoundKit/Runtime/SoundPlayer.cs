using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using SoundKit.Obsolute;
using UnityEngine;
using UnityEngine.Audio;

namespace SoundKit
{
    [AddComponentMenu("")]
    internal class SoundPlayer : MonoBehaviour, ISoundHandler
    {
        private static readonly Queue<SoundPlayer> s_pool = new();
        private AudioSource _audioSource;
        private bool _releaseOnStop;
        public int InstanceNo { get; private set; }

        public ISoundHandler Play(float volume)
        {
            _audioSource.volume = volume;
            _audioSource.Play();
            return this;
        }

        public ISoundHandler SetVolume(float volume)
        {
            _audioSource.volume = volume;
            return this;
        }

        public ISoundHandler SetPitch(float pitch)
        {
            _audioSource.pitch = pitch;
            return this;
        }

        public ISoundHandler SetLoop(bool loop)
        {
            _audioSource.loop = loop;
            return this;
        }

        public async UniTask TweenVolumeUpAsync(float duration, float volume,
            CancellationToken cancellationToken = default)
        {
            var linkedTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, destroyCancellationToken);
            await _audioSource.TweenVolumeUpAsync(duration, volume, linkedTokenSource.Token);
        }

        public async UniTask TweenVolumeDownAsync(float duration, float volume,
            CancellationToken cancellationToken = default)
        {
            var linkedTokenSource =
                CancellationTokenSource.CreateLinkedTokenSource(cancellationToken, destroyCancellationToken);
            await _audioSource.TweenVolumeDownAsync(duration, volume, linkedTokenSource.Token);
        }

        public ISoundHandler Stop()
        {
            _audioSource.Stop();
            if (_releaseOnStop)
                Release();
            return this;
        }

        public void Release()
        {
            s_pool.Enqueue(this);
            gameObject.SetActive(false);
        }


        internal ISoundHandler Initialize(AudioClip clip, AudioMixerGroup group = null, bool releaseOnStop = true)
        {
            if (_audioSource == null)
                _audioSource = gameObject.AddComponent<AudioSource>();

            // 初期設定
            _audioSource.outputAudioMixerGroup = group;
            _audioSource.clip = clip;

            // 初期化
            _audioSource.playOnAwake = false;
            _audioSource.loop = false;
            _audioSource.pitch = 1f;

            _releaseOnStop = releaseOnStop;

            InstanceNo++;
            gameObject.SetActive(true);
            return this;
        }

        public static SoundPlayer GetOrCreate(Transform parent)
        {
            if (s_pool.TryDequeue(out var player))
            {
                player.gameObject.SetActive(true);
                return player;
            }

            player = new GameObject("SoundPlayer").AddComponent<SoundPlayer>();
            player.transform.SetParent(parent, false);
            return player;
        }
    }
}