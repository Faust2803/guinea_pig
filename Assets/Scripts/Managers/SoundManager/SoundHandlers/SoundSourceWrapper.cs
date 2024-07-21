using System;
using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers.SoundManager.SoundHandlers
{
    public class SoundSourceWrapper : ISoundSourcePlayer
    {
        public event Action<ISoundSourceInfo> OnCompleted;
        public SoundId Id { get; }
        public string Key { get; }
        public bool IsPlaying => _isValid && _player.IsPlaying;
        public bool IsStopped => !_isValid || _player.IsStopped;
        
        private bool _isValid;
        
        private ISoundSourcePlayer _player;

        public SoundSourceWrapper(ISoundSourcePlayer player)
        {
            _player = player;
            Id = player.Id;
            Key = player.Key;
            _isValid = true;
            _player.OnCompleted += Completed;
        }
        
        public void Play(bool immediately = false)
        {
            if(_isValid)
                _player.Play(immediately);
        }

        public void Pause(bool immediately = false)
        {
            if(_isValid)
                _player.Pause(immediately);
        }

        public void Stop(bool immediately = false)
        {
            if(_isValid)
                _player.Stop(immediately);
        }

        public void Restart(bool immediately = false)
        {
            if(_isValid)
                _player.Restart(immediately);
        }

        public ISoundSourcePlayer SetLoop(bool isLoop)
        {
            if(_isValid)
                _player.SetLoop(isLoop);

            return this;
        }

        public ISoundSourcePlayer SetIgnoreListenerPause(bool isIgnore)
        {
            if(_isValid)
                _player.SetLoop(isIgnore);

            return this;
        }

        public ISoundSourcePlayer WithStartSmooth(SoundSmoothEffect effect)
        {
            if(_isValid)
                _player.WithStartSmooth(effect);

            return this;
        }

        public ISoundSourcePlayer WithStopSmooth(SoundSmoothEffect effect)
        {
            if(_isValid)
                _player.WithStopSmooth(effect);

            return this;
        }

        public ISoundSourcePlayer SetMixerGroup(AudioMixerGroup mixerGroup)
        {
            if (_isValid)
                _player.SetMixerGroup(mixerGroup);

            return this;
        }

        public ISoundSourcePlayer FollowAt(I3DSoundPoint target)
        {
            if(_isValid)
                _player.FollowAt(target);

            return this;
        }

        public ISoundSourcePlayer SetPosition(Vector3 position)
        {
            if(_isValid)
                _player.SetPosition(position);

            return this;
        }

        private void Completed(ISoundSourceInfo info)
        {
            _isValid = false;
            OnCompleted?.Invoke(this);
            info.OnCompleted -= Completed;
        }
    }
}