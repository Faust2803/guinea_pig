using System.Collections.Generic;
using Managers.SoundManager.Enums;
using UnityEngine.Audio;

namespace Managers.SoundManager.Base
{
    public interface ISoundManager
    {
        void PlaySound(SoundId soundId, bool isLoop = false, bool isPaused = true, bool restart = false);
        void PlaySound(string soundId, bool isLoop = false, bool isPaused = true, bool restart = false);
        void PlaySound(SoundId soundId, SoundSmoothEffect startSmoothEffect, SoundSmoothEffect stopSmoothEffect, bool isLoop = false, bool isPaused = true, bool restart = false);
        void PlaySound(string soundId, SoundSmoothEffect startSmoothEffect, SoundSmoothEffect stopSmoothEffect, bool isLoop = false, bool isPaused = true, bool restart = false);
        void PauseSound(SoundId soundId, bool immediately = false);
        void PauseSound(string soundId, bool immediately = false);
        void StopSound(SoundId soundId, bool immediately = false);
        void StopSound(string soundId, bool immediately = false);
        bool TryGetSoundInfo(SoundId soundId, out ISoundSourceInfo info);
        bool TryGetSoundInfo(string soundId, out ISoundSourceInfo info);
        bool IsPlaying(SoundId soundId);
        void PlayOneShot(SoundId soundId, bool isPaused = true);
        void PlayOneShot(string soundId, bool isPaused = true);
        void PlayOneShot(SoundId soundId, SoundSmoothEffect startSmoothEffect, SoundSmoothEffect stopSmoothEffect, bool isPaused = true);
        void PlayOneShot(string soundId, SoundSmoothEffect startSmoothEffect, SoundSmoothEffect stopSmoothEffect, bool isPaused = true);
        bool TryGetOneShotInfo(SoundId soundId, out IEnumerable<ISoundSourceInfo> info);
        bool TryGetOneShotInfo(string soundId, out IEnumerable<ISoundSourceInfo> info);
        ISoundSourcePlayer GetSoundPlayer(SoundId soundId, bool is3D = false);
        ISoundSourcePlayer GetSoundPlayer(string soundId, bool is3D = false);
        bool TryGetSoundPlayerInfos(SoundId soundId, out IEnumerable<ISoundSourceInfo> info);
        bool TryGetSoundPlayerInfos(string soundId, out IEnumerable<ISoundSourceInfo> info);
        void StopAllSounds(bool immediately = false);
        void StopAllOneShots(bool immediately = false);
        void StopAllExternalPlayers(bool immediately = false);
        void StopAllSources(bool immediately = false);
        AudioMixerGroup GetMixerGroup(SoundType type);
        SoundManager.SoundVolumeSetting[] GetVolumeSettings();
        void UpdateVolume(SoundType soundType, float value);
        void UpdateVolumeSound(SoundId soundId, float value);
    }
}