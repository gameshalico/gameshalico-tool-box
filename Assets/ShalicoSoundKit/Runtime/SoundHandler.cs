﻿using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UniRx;

namespace ShalicoSoundKit
{
    internal class SoundHandler : ISoundHandler
    {
        private readonly SoundPlayer _soundPlayer;

        internal SoundHandler(SoundPlayer soundPlayer)
        {
            _soundPlayer = soundPlayer;
            SoundGroupID = 0;
        }

        public bool IsValid => _soundPlayer.CurrentHandler == this;
        public int SoundGroupID { get; private set; }

        public ISoundHandler Play()
        {
            if (!IsValid)
                throw new InvalidOperationException("This sound handler is already released.");

            _soundPlayer.Play();
            return this;
        }

        public ISoundHandler Stop()
        {
            if (!IsValid)
                return this;

            _soundPlayer.Stop();
            return this;
        }

        public void Release()
        {
            if (!IsValid)
                return;

            _soundPlayer.Release();
        }

        public ISoundHandler SetVolume(float volume)
        {
            if (!IsValid)
                throw new InvalidOperationException("This sound handler is already released.");

            _soundPlayer.SetVolume(volume);
            return this;
        }

        public ISoundHandler SetPitch(float pitch)
        {
            if (!IsValid)
                throw new InvalidOperationException("This sound handler is already released.");

            _soundPlayer.SetPitch(pitch);
            return this;
        }

        public ISoundHandler SetLoop(bool loop)
        {
            if (!IsValid)
                throw new InvalidOperationException("This sound handler is already released.");

            _soundPlayer.SetLoop(loop);
            return this;
        }

        public ISoundHandler SetGroupID(int soundGroupID)
        {
            if (!IsValid)
                throw new InvalidOperationException("This sound handler is already released.");

            SoundGroupID = soundGroupID;
            return this;
        }

        public async UniTask TweenVolumeUpAsync(float duration, float volume,
            CancellationToken cancellationToken = default)
        {
            if (!IsValid)
                throw new InvalidOperationException("This sound handler is already released.");
            await _soundPlayer.TweenVolumeUpAsync(duration, volume, cancellationToken);
        }

        public async UniTask TweenVolumeDownAsync(float duration, float volume,
            CancellationToken cancellationToken = default)
        {
            if (!IsValid)
                throw new InvalidOperationException("This sound handler is already released.");
            await _soundPlayer.TweenVolumeDownAsync(duration, volume, cancellationToken);
        }

        public async UniTask PlayAsync(CancellationToken cancellationToken = default)
        {
            if (!IsValid)
                throw new InvalidOperationException("This sound handler is already released.");

            _soundPlayer.Play();
            try
            {
                await _soundPlayer.ReleasedAsObservable.Take(1).ToUniTask(cancellationToken: cancellationToken);
            }
            catch (OperationCanceledException e)
            {
                if (IsValid)
                    Release();

                throw;
            }
        }
    }
}