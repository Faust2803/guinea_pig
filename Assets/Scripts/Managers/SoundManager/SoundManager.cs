using System;
using System.Collections.Generic;
using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using Managers.SoundManager.SoundHandlers;
using UnityEngine;
using UnityEngine.Audio;
using Zenject;

namespace Managers.SoundManager
{
    public class SoundManager : MonoBehaviour, ISoundManager
    {
        [SerializeField] private AudioMixerGroup _defaultMixerGroup;
        [SerializeField] private MixerGroupData[] _mixerGroupData = Array.Empty<MixerGroupData>();
        [SerializeField] private Transform _soundSourceParent;
        [SerializeField, Range(5, 100)] private int _maxCountOfSource = 15;
        [SerializeField, Range(5, 100)] private int _maxCountOfExternalSource = 10;

        [Inject] private ISoundSourceHandlerFactory _factory;
        [Inject] private ISoundDataProvider _dataProvider;
        
        private LinkedList<ISoundSourceHandler> _activeSoundHandlers = new LinkedList<ISoundSourceHandler>();
        private LinkedList<ISoundSourceHandler> _activeOneShotHandlers = new LinkedList<ISoundSourceHandler>();
        private Queue<ISoundSourceHandler> _freeSoundHandlers = new Queue<ISoundSourceHandler>();

        private LinkedList<ISoundSourceHandler> _activeExternalSoundHandlers = new LinkedList<ISoundSourceHandler>();
        private Queue<ISoundSourceHandler> _freeExternalSoundHandlers = new Queue<ISoundSourceHandler>();

        private List<ISoundSourceHandler> _handlersToDeleteList = new List<ISoundSourceHandler>();

        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
            foreach (var data in _mixerGroupData)
                SetVolume(data, GetSavedVolume(data.SoundType));
        }

        public bool IsPlaying (SoundId sound)
        {
            if(TryFindHandler(sound, _activeSoundHandlers, out var handler) && handler.IsPlaying)
                return true;
            return false;
        }

        public void PlaySound(SoundId soundId, bool isLoop = false, bool isPaused = true, bool restart = false)
        {
            if (_dataProvider.TryGetData(soundId, out var data))
                PlaySound(data, isLoop, isPaused, restart);
        }
        
        public void PlaySound(string soundId, bool isLoop = false, bool isPaused = true, bool restart = false)
        {
            if (_dataProvider.TryGetData(soundId, out var data))
                PlaySound(data, isLoop, isPaused, restart);
        }
        private void PlaySound(ISoundData data, bool isLoop = false, bool isPaused = true, bool restart = false)
        {
            if (TryFindHandler(data.IdInfo.Id, _activeSoundHandlers, out var handler) && handler.IsValid)
            {
                if (restart)
                {
                    handler.Restart();

                    return;
                }

                handler.SourceVolume = 1f;
                handler.Play();

                return;
            }

            if (!TryGetHandler(data, false, false, out handler)) return;

            handler.SetMixerGroup(GetMixerGroup(data.SoundType))
                   .SetLoop(isLoop)
                   .SetIgnoreListenerPause(!isPaused);

            _activeSoundHandlers.AddLast(handler);
            handler.SourceVolume = 1f;
            handler.Play();
        }

        public void PlaySound(SoundId soundId, SoundSmoothEffect startSmoothEffect, SoundSmoothEffect stopSmoothEffect,
                              bool isLoop = false, bool isPaused = true, bool restart = false)
        {
            if (_dataProvider.TryGetData(soundId, out var data))
                PlaySound(data, startSmoothEffect, stopSmoothEffect, isLoop, isPaused, restart);
        }

        public void PlaySound(string soundId, SoundSmoothEffect startSmoothEffect, SoundSmoothEffect stopSmoothEffect,
                              bool isLoop = false, bool isPaused = true, bool restart = false)
        {
            if (!_dataProvider.TryGetData(soundId, out var data))
                PlaySound(data, startSmoothEffect, stopSmoothEffect, isLoop, isPaused, restart);
        }
        private void PlaySound(ISoundData data, SoundSmoothEffect startSmoothEffect, SoundSmoothEffect stopSmoothEffect, 
                               bool isLoop = false, bool isPaused = true, bool restart = false)
        {
            if (TryFindHandler(data.IdInfo.Id, _activeSoundHandlers, out var handler))
            {
                handler.WithStartSmooth(startSmoothEffect);
                handler.WithStopSmooth(stopSmoothEffect);

                if (restart)
                {
                    handler.Restart();

                    return;
                }

                handler.Play();

                return;
            }

            if (!TryGetHandler(data, false, false, out handler)) return;

            handler.SetMixerGroup(GetMixerGroup(data.SoundType))
                   .SetLoop(isLoop)
                   .SetIgnoreListenerPause(!isPaused)
                   .WithStartSmooth(startSmoothEffect)
                   .WithStopSmooth(stopSmoothEffect);

            _activeSoundHandlers.AddLast(handler);
            handler.Play();
        }

        public void PauseSound(SoundId soundId, bool immediately = false)
        {
            if (TryFindHandler(soundId, _activeSoundHandlers, out var handler))
                handler.Pause(immediately);
        }

        public void PauseSound(string soundId, bool immediately = false)
        {
            if(_dataProvider.TryGetData(soundId, out var data))
                PauseSound(data.IdInfo.Id, immediately);
        }

        public void StopSound(SoundId soundId, bool immediately = false)
        {
            if (TryFindHandler(soundId, _activeSoundHandlers, out var handler))
                handler.Stop(immediately);
        }

        public void StopSound(string soundId, bool immediately = false)
        {
            if(_dataProvider.TryGetData(soundId, out var data))
                StopSound(data.IdInfo.Id, immediately);
        }

        public void PlayOneShot(SoundId soundId, bool isPaused = true)
        {
            if (_dataProvider.TryGetData(soundId, out var data))
                PlayOneShot(data, isPaused);
        }
        
        public void PlayOneShot(string soundId, bool isPaused = true)
        {
            if (_dataProvider.TryGetData(soundId, out var data))
                PlayOneShot(data, isPaused);
        }
        
        private void PlayOneShot(ISoundData data, bool isPaused = true)
        {
            if (!TryGetHandler(data, false, false, out var handler)) return;

            handler.SetMixerGroup(GetMixerGroup(data.SoundType))
                   .SetIgnoreListenerPause(!isPaused)
                   .SetLoop(false);

            _activeOneShotHandlers.AddLast(handler);

            handler.SourceVolume = 1f;
            handler.Play();
        }

        public void PlayOneShot(SoundId soundId, SoundSmoothEffect startSmoothEffect,
                                SoundSmoothEffect stopSmoothEffect, bool isPaused = true)
        {
            if (_dataProvider.TryGetData(soundId, out var data))
                PlayOneShot(data, startSmoothEffect, stopSmoothEffect, isPaused);
        }
        
        public void PlayOneShot(string soundId, SoundSmoothEffect startSmoothEffect, SoundSmoothEffect stopSmoothEffect,
                                bool isPaused = true)
        {
            if (_dataProvider.TryGetData(soundId, out var data))
                PlayOneShot(data, startSmoothEffect, stopSmoothEffect, isPaused);
        }
        private void PlayOneShot(ISoundData data, SoundSmoothEffect startSmoothEffect, SoundSmoothEffect stopSmoothEffect, bool isPaused = true)
        {
            if (!TryGetHandler(data, false, false, out var handler)) return;

            handler.SetMixerGroup(GetMixerGroup(data.SoundType))
                   .SetIgnoreListenerPause(!isPaused)
                   .SetLoop(false);

            _activeOneShotHandlers.AddLast(handler);

            handler.Play();
        }

        public ISoundSourcePlayer GetSoundPlayer(SoundId soundId, bool is3D = false)
        {
            return _dataProvider.TryGetData(soundId, out var data) ? GetSoundPlayer(data, is3D) : new SoundSourceDummy(soundId, "none");
        }
        
        public ISoundSourcePlayer GetSoundPlayer(string soundId, bool is3D = false)
        {
            return _dataProvider.TryGetData(soundId, out var data) ? GetSoundPlayer(data, is3D) : new SoundSourceDummy((SoundId) 404, soundId);
        }
        
        private ISoundSourcePlayer GetSoundPlayer(ISoundData data, bool is3D = false)
        {
            if (!TryGetHandler(data, true, is3D, out var handler)) return new SoundSourceDummy(data.IdInfo.Id, data.IdInfo.Key);

            handler.SetMixerGroup(GetMixerGroup(data.SoundType));
            _activeExternalSoundHandlers.AddLast(handler);

            return new SoundSourceWrapper(handler);
        }

        public void StopAllSounds(bool immediately = false)
        {
            foreach (var handler in _activeSoundHandlers)
                handler.Stop(immediately);
        }

        public void StopAllOneShots(bool immediately = false)
        {
            foreach (var handler in _activeOneShotHandlers)
                handler.Stop(immediately);
        }

        public void StopAllExternalPlayers(bool immediately = false)
        {
            foreach (var handler in _activeExternalSoundHandlers)
                handler.Stop(immediately);
        }

        public void StopAllSources(bool immediately = false)
        {
            StopAllSounds(immediately);
            StopAllOneShots(immediately);
            StopAllExternalPlayers(immediately);
        }
        
        public bool TryGetSoundInfo(SoundId soundId, out ISoundSourceInfo info)
        {
            var result = TryFindHandler(soundId, _activeSoundHandlers, out var handler);
            info = handler;
            return result;
        }
        
        public bool TryGetSoundInfo(string soundId, out ISoundSourceInfo info)
        {
            info = null;
            return _dataProvider.TryGetData(soundId, out var data) && TryGetSoundInfo(data.IdInfo.Id, out info);
        }
        
        public bool TryGetOneShotInfo(SoundId soundId, out IEnumerable<ISoundSourceInfo> info)
        {
            return TryGetHandlerInfos(soundId, _activeOneShotHandlers, out info);
        }
        
        public bool TryGetOneShotInfo(string soundId, out IEnumerable<ISoundSourceInfo> info)
        {
            info = null;
            return _dataProvider.TryGetData(soundId, out var data) && TryGetOneShotInfo(data.IdInfo.Id, out info);
        }

        public bool TryGetSoundPlayerInfos(string soundId, out IEnumerable<ISoundSourceInfo> info)
        {
            info = null;
            return _dataProvider.TryGetData(soundId, out var data) && TryGetSoundPlayerInfos(data.IdInfo.Id, out info);
        }
        public bool TryGetSoundPlayerInfos(SoundId soundId, out IEnumerable<ISoundSourceInfo> info)
        {
            return TryGetHandlerInfos(soundId, _activeExternalSoundHandlers, out info);
        }

        private bool TryGetHandlerInfos(SoundId soundId, IEnumerable<ISoundSourceHandler> collection, out IEnumerable<ISoundSourceInfo> result)
        {
            var handlers = new List<ISoundSourceInfo>();

            foreach (var handler in collection)
            {
                if (handler.Id == soundId)
                    handlers.Add(handler);
            }

            result = handlers;

            return handlers.Count > 0;
        }

        private bool TryGetHandler(ISoundData data, bool isExternal, bool is3D, out ISoundSourceHandler handler)
        {
            handler = null;
            
            var freeHandlers = isExternal ? _freeExternalSoundHandlers : _freeSoundHandlers;

            if (freeHandlers.TryDequeue(out handler))
            {
                handler.Setup(data, is3D);
                return true;
            }

            var limitSourceCount = isExternal ? _maxCountOfExternalSource : _maxCountOfSource;
            var currentSourceCount = isExternal ? _activeExternalSoundHandlers.Count : _activeSoundHandlers.Count + _activeOneShotHandlers.Count;

            if (limitSourceCount <= currentSourceCount) return false;
            
            handler = _factory.Create(data, _soundSourceParent, isExternal, is3D);
            return true;
        }

        public AudioMixerGroup GetMixerGroup(SoundType type)
        {
            foreach (var groupData in _mixerGroupData)
            {
                if (groupData.SoundType != type) continue;

                return groupData.MixerGroup;
            }

            Debug.LogError($"[{nameof(SoundManager)}] Audio mixer group for sound type {type} is not declared.");

            return _defaultMixerGroup;
        }

        private void Update()
        {
            UpdateAndRemoveCompletedHandlers(_activeSoundHandlers, _freeSoundHandlers);
            UpdateAndRemoveCompletedHandlers(_activeOneShotHandlers, _freeSoundHandlers);
            UpdateAndRemoveCompletedHandlers(_activeExternalSoundHandlers, _freeExternalSoundHandlers);
        }

        private void UpdateAndRemoveCompletedHandlers(ICollection<ISoundSourceHandler> handlers, Queue<ISoundSourceHandler> freeHandlers)
        {
            foreach (var handler in handlers)
            {
                handler.Update();

                if (!handler.IsValid)
                    _handlersToDeleteList.Add(handler);
            }
            
            if(_handlersToDeleteList.Count == 0) return;

            foreach (var handler in _handlersToDeleteList)
            {
                handlers.Remove(handler);
                handler.Reset();
                freeHandlers.Enqueue(handler);
            }

            _handlersToDeleteList.Clear();
        }

        private bool TryFindHandler(SoundId id, IEnumerable<ISoundSourceHandler> handlers, out ISoundSourceHandler handler)
        {
            handler = null;
            var isFound = false;

            foreach (var item in handlers)
            {
                if (item.Id != id) continue;

                isFound = true;
                handler = item;

                break;
            }

            return isFound;
        }

        public void UpdateVolume(SoundType soundType, float value)
        {
            value = Mathf.Clamp(value, 0.001f, 1f);
            SaveVolume(soundType, value);

            foreach (var mixerGroupData in _mixerGroupData)
            {
                if (mixerGroupData.SoundType != soundType) continue;

                SetVolume(mixerGroupData, value);

                break;
            }
        }

        public void UpdateVolumeSound(SoundId soundId, float value)
        {
            value = Mathf.Clamp(value, 0.001f, 1f);
            if (TryFindHandler(soundId, _activeSoundHandlers, out var handler) && handler.IsValid)
                handler.SourceVolume = value;
        }

        public SoundVolumeSetting[] GetVolumeSettings()
        {
            var result = new SoundVolumeSetting[_mixerGroupData.Length];

            for (int i = 0; i < _mixerGroupData.Length; i++)
            {
                var data = _mixerGroupData[i];
                // var value = Mathf.Pow((float) Math.E, Mathf.Abs(GetSavedVolume(data.SoundType) / 20f));
                var value = GetSavedVolume(data.SoundType);
                result[i] = new SoundVolumeSetting(data.SoundType, data.SettingName, Mathf.Clamp01(value));
            }

            return result;
        }

        private void SetVolume(MixerGroupData data, float value)
        {
            var volume = Mathf.Log(value) * 20f;
            data.MixerGroup.audioMixer.SetFloat(data.VolumeValueName, volume);
        }

        private void SaveVolume(SoundType soundType, float volume)
        {
            PlayerPrefs.SetFloat(soundType.ToString(), volume - 1f);
        }

        private float GetSavedVolume(SoundType soundType)
        {
            return PlayerPrefs.GetFloat(soundType.ToString()) + 1f;
        }

        private Queue<ISoundSourceHandler> _pausedQueue = new Queue<ISoundSourceHandler>();
        private bool _isApplicationPaused;
        
        private void OnApplicationPause(bool pauseStatus)
        {
            if(_isApplicationPaused == pauseStatus) return;

            _isApplicationPaused = pauseStatus;
            
            if (pauseStatus)
            {
                AddToPausedHandlers(_activeSoundHandlers);
                AddToPausedHandlers(_activeOneShotHandlers);
                AddToPausedHandlers(_activeExternalSoundHandlers);
                return;
            }

            while (_pausedQueue.TryDequeue(out var handler))
            {
                if(handler.IsValid && !handler.IsStopped && !handler.IsPlaying)
                    handler.Play(true);
            }
        }

        private void AddToPausedHandlers(IEnumerable<ISoundSourceHandler> handlers)
        {
            foreach (var handler in handlers)
            {
                if(!handler.IsValid || handler.IsStopped || !handler.IsPlaying) continue;
                    
                _pausedQueue.Enqueue(handler);
                handler.Pause(true);
            }
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(SetAppPause))]
        private void SetAppPause()
        {
            OnApplicationPause(true);
        }

        [ContextMenu(nameof(SetAppUnpause))]
        private void SetAppUnpause()
        {
            OnApplicationPause(false);
        }
#endif

        private void OnDisable()
        {
            StopAllSources();
        }

        [Serializable]
        private class MixerGroupData
        {
            public SoundType SoundType;
            public AudioMixerGroup MixerGroup;
            public string VolumeValueName;
            public string SettingName;
        }

        public class SoundVolumeSetting
        {
            public readonly SoundType SoundType;
            public readonly float Value;
            public readonly string Name;

            public SoundVolumeSetting(SoundType soundType, string name, float value)
            {
                SoundType = soundType;
                Value = value;
                Name = name;
            }
        }
    }
}