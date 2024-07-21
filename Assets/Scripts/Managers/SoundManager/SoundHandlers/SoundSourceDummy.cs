using System;
using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers.SoundManager.SoundHandlers
{
    public class SoundSourceDummy : ISoundSourcePlayer
    {
        public event Action<ISoundSourceInfo> OnCompleted;
        public SoundId Id { get; }
        public string Key { get; }
        public bool IsPlaying => false;
        public bool IsStopped => true;

        public SoundSourceDummy(SoundId id, string key)
        {
            Id = id;
            Key = key;
        }

        public void Play(bool immediately = false){}

        public void Pause(bool immediately = false){}

        public void Stop(bool immediately = false){}
        public void Restart(bool immediately = false){}

        public ISoundSourcePlayer SetLoop(bool isLoop)
        {
            return this;
        }

        public ISoundSourcePlayer SetIgnoreListenerPause(bool isIgnore)
        {
            return this;
        }

        public ISoundSourcePlayer WithStartSmooth(SoundSmoothEffect effect)
        {
            return this;
        }

        public ISoundSourcePlayer WithStopSmooth(SoundSmoothEffect effect)
        {
            return this;
        }

        public ISoundSourcePlayer SetMixerGroup(AudioMixerGroup mixerGroup)
        {
            return this;
        }

        public ISoundSourcePlayer FollowAt(I3DSoundPoint target)
        {
            return this;
        }

        public ISoundSourcePlayer SetPosition(Vector3 position)
        {
            return this;
        }
    }
}