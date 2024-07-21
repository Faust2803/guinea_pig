using System;
using Managers.SoundManager.Enums;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers.SoundManager.Base
{
    public interface ISoundSourceInfo
    {
        event Action<ISoundSourceInfo> OnCompleted;
        SoundId Id { get; }
        string Key { get; }
        bool IsPlaying { get; }
        bool IsStopped { get; }
    }

    public interface ISoundSourcePlayer : ISoundSourceInfo
    {
        void Play(bool immediately = false);
        void Pause(bool immediately = false);
        void Stop(bool immediately = false);
        void Restart(bool immediately = false);
        
        ISoundSourcePlayer SetLoop(bool isLoop);
        ISoundSourcePlayer SetIgnoreListenerPause(bool isIgnore);
        ISoundSourcePlayer WithStartSmooth(SoundSmoothEffect effect);
        ISoundSourcePlayer WithStopSmooth(SoundSmoothEffect effect);
        ISoundSourcePlayer SetMixerGroup(AudioMixerGroup mixerGroup);
        ISoundSourcePlayer FollowAt(I3DSoundPoint target);
        ISoundSourcePlayer SetPosition(Vector3 position);
    }
    
    public interface ISoundSourceHandler : ISoundSourcePlayer
    {
        bool IsValid { get; }
        void Setup(ISoundData data, bool is3D = false);
        void Update();
        void Reset();
        float SourceVolume { get; set; }
    }
}