using System;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.Audio;

namespace ShalicoSoundKit
{
    [AddComponentMenu("")]
    internal class SoundPlayer : MonoBehaviour
    {
        private static readonly Queue<SoundPlayer> s_pool = new();
        private readonly Subject<Unit> _releasedSubject = new();

        private AudioSource _audioSource;

        private bool _isDestroyed;
        private bool _releaseOnStop;
        private CancellationTokenSource _tweenCancellationTokenSource;
        public SoundHandler CurrentHandler { get; private set; }
        public IObservable<Unit> ReleasedAsObservable => _releasedSubject;

        private void Update()
        {
            if (!_audioSource.isPlaying && _releaseOnStop)
                Release();
        }

        private void OnDestroy()
        {
            _isDestroyed = true;
            s_pool.Clear();
        }

        public void Play()
        {
            _audioSource.Play();
        }

        public void SetVolume(float volume)
        {
            _audioSource.volume = volume;
        }

        public void SetPitch(float pitch)
        {
            _audioSource.pitch = pitch;
        }

        public void SetLoop(bool loop)
        {
            _audioSource.loop = loop;
        }

        public async UniTask TweenVolumeUpAsync(float duration, float volume,
            CancellationToken cancellationToken = default)
        {
            await _audioSource.TweenVolumeUpAsync(duration, volume,
                CancelAndCreateTweenLinkedToken(cancellationToken));
        }

        public async UniTask TweenVolumeDownAsync(float duration, float volume,
            CancellationToken cancellationToken = default)
        {
            await _audioSource.TweenVolumeDownAsync(duration, volume,
                CancelAndCreateTweenLinkedToken(cancellationToken));
        }

        public void Stop()
        {
            _audioSource.Stop();
            if (_releaseOnStop)
                Release();
        }

        public void Release()
        {
            if (_isDestroyed)
                return;

            s_pool.Enqueue(this);
            gameObject.SetActive(false);
            SoundManager.UnregisterHandler(CurrentHandler);
            CurrentHandler = null;
            _releasedSubject.OnNext(Unit.Default);
        }

        private CancellationToken CancelAndCreateTweenLinkedToken(CancellationToken cancellationToken)
        {
            _tweenCancellationTokenSource?.Cancel();
            _tweenCancellationTokenSource = new CancellationTokenSource();
            return CancellationTokenSource.CreateLinkedTokenSource(cancellationToken,
                _tweenCancellationTokenSource.Token,
                this.GetCancellationTokenOnDestroy()).Token;
        }


        internal ISoundHandler Initialize(AudioClip clip, AudioMixerGroup group = null, bool releaseOnStop = true)
        {
            _tweenCancellationTokenSource?.Cancel();
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

            gameObject.SetActive(true);
            CurrentHandler = new SoundHandler(this);
            SoundManager.RegisterHandler(CurrentHandler);

            return CurrentHandler;
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