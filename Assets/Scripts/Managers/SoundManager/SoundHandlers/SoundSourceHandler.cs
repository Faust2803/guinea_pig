using System;
using DG.Tweening;
using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UnityEngine;
using UnityEngine.Audio;

namespace Managers.SoundManager.SoundHandlers
{
    public class SoundSourceHandler : ISoundSourceHandler
    {
        public event Action<ISoundSourceInfo> OnCompleted;
        public string Key => _data.IdInfo.Key;
        public bool IsPlaying => _state == SoundSourceState.Play;
        public SoundId Id => _data.IdInfo.Id;
        public bool IsValid => _state != SoundSourceState.Empty;
        public bool IsStopped => _state == SoundSourceState.Empty || _state == SoundSourceState.Stop;

        private static readonly SoundSourceState _playFilter = SoundSourceState.Empty | SoundSourceState.Stop;// | SoundSourceState.Play;

        private static readonly SoundSourceState _pauseFilter = SoundSourceState.Empty | SoundSourceState.Ready |
                                                                SoundSourceState.Stop;// | SoundSourceState.Pause;

        private static readonly SoundSourceState _restartFilter = SoundSourceState.Empty | SoundSourceState.Stop; //| SoundSourceState.Transition;
        
        private static readonly SoundSourceState _stopFilter = SoundSourceState.Empty | SoundSourceState.Ready;// | SoundSourceState.Stop;
        
        private static readonly SoundSourceState _updateFilter = SoundSourceState.Pause | SoundSourceState.Transition | 
                                                                 SoundSourceState.Stop  | SoundSourceState.Ready;

        private AudioSource _source;
        private ISoundData _data;
        private Transform _transform;
        
        private bool _isValid;
        private bool _isPauseWhenTransition;
        
        private bool _isIgnoreListenerPause;
        private bool _listenerPaused => !_isIgnoreListenerPause && AudioListener.pause;
        
        private bool _isHaveTargetPoint;
        private I3DSoundPoint _point;
        
        private SoundSmoothEffect _defaultEffect;
        private SoundSmoothEffect _startEffect;
        private SoundSmoothEffect _endEffect;

        private Sequence _sequence;
        private SoundSourceState _state;

        public SoundSourceHandler(AudioSource source, ISoundData data, Transform transform, bool is3D = false)
        {
            _source = source;
            _defaultEffect = new SoundSmoothEffect(0f);
            _startEffect = _defaultEffect;
            _endEffect = _defaultEffect;
            _transform = transform;
            _state = SoundSourceState.Empty;
            Setup(data, is3D);
        }

        public float SourceVolume
        {
            get => _source.volume;
            set => _source.volume = value;
        }

        public void Setup(ISoundData data, bool is3D = false)
        {
            if (_state != SoundSourceState.Empty)
            {
                Debug.LogError($"[{GetType().Name}] This sound source handle sound with type: {data.SoundType} currently. Player status: {_state.ToString()}");
                return;
            }

            _data = data;
            _source.clip = _data.Clip;
            _data.Settings.Setup(_source, is3D);

            _isValid = true;
            _state = SoundSourceState.Ready;
            _source.enabled = true;
        }

        public void Play(bool immediately = false)
        {
            if((_state | _playFilter) == _playFilter) return;

            if (_state == SoundSourceState.Play)
            {
                if (immediately)
                {
                    _sequence?.Kill();
                    _source.volume = _data.Settings.Volume;
                }
                
                return;
            }

            if (_state == SoundSourceState.Transition)
            {
                if (!_sequence.IsPlaying())
                {
                    _source.UnPause();
                    _sequence.Play();
                }
               
                _isPauseWhenTransition = false;
                return;
            }

            var oldState = _state;

            _state = SoundSourceState.Play;
            _isPauseWhenTransition = false;

            _sequence?.Kill();

            if (immediately)
            {
                _source.volume = _data.Settings.Volume;
                
                if(oldState == SoundSourceState.Pause)
                    _source.UnPause();
                else 
                    _source.Play();
                
                return;
            }
            
            if (_startEffect.IsValid)
            {
                if (Mathf.Approximately(_source.volume, _data.Settings.Volume))
                    _source.volume = 0f;
                
                _sequence = DOTween.Sequence();
                _sequence = _startEffect.ApplyEffect(_sequence, _source, _data.Settings.Volume);
                _sequence.SetUpdate(_isIgnoreListenerPause);
            }
            else
                _source.volume = _data.Settings.Volume;

            if (!_source.isPlaying)
            {
                if(oldState == SoundSourceState.Pause)
                    _source.UnPause();
                else 
                    _source.Play();
            } 
        }

        public void Restart(bool immediately = false)
        {
            if (_state == SoundSourceState.Ready)
            {
                Play(immediately);
                return;
            }
            
            if((_state | _restartFilter) == _restartFilter) return;

            if (immediately)
            {
                _sequence?.Kill();
                _source.Stop();
                _source.Play();
                _source.volume = _data.Settings.Volume;
                _state = SoundSourceState.Play;
                
                return;
            }
            
            if(_state == SoundSourceState.Transition) return;

            _sequence?.Kill();
            
            _state = SoundSourceState.Transition;
            _sequence = DOTween.Sequence();

            if (_endEffect.IsValid)
                _sequence = _endEffect.ApplyEffect(_sequence, _source, 0f);

            _sequence.AppendCallback((() =>
            {
                _source.Stop();
                
                if(!_isPauseWhenTransition)
                    _source.Play();

                _state = _isPauseWhenTransition ? SoundSourceState.Pause : SoundSourceState.Play;
                _source.volume = _startEffect.IsValid ? 0f : _data.Settings.Volume;
                    
                _isPauseWhenTransition = false;
            }));

            if (_startEffect.IsValid && !_isPauseWhenTransition)
                _sequence = _startEffect.ApplyEffect(_sequence, _source, 0f, _data.Settings.Volume);
            
            _sequence.SetUpdate(_isIgnoreListenerPause);
        }

        public void Pause(bool immediately = false)
        {
            if((_state | _pauseFilter) == _pauseFilter) return;

            if (_state == SoundSourceState.Pause)
            {
                if (immediately)
                {
                    _sequence?.Kill();
                    _source.Pause();
                    _source.volume = _data.Settings.Volume;
                }
                
                return;
            }

            if (_state == SoundSourceState.Transition)
            {
                _isPauseWhenTransition = true;
                _sequence.Pause();
                _source.Pause();
                
                return;
            }
            
            _state = SoundSourceState.Pause;
            _sequence?.Kill();

            if (_endEffect.IsValid)
            {
                _sequence = DOTween.Sequence();
                _sequence = _endEffect.ApplyEffect(_sequence, _source, 0f).AppendCallback((() => _source.Pause()));
                _sequence.SetUpdate(_isIgnoreListenerPause);
            }
            else
            {
                _source.Pause();
                _source.volume = _data.Settings.Volume;
            }
        }

        public void Update()
        {
            var isSoundSourceWork = _source.isPlaying || _listenerPaused || (_state | _updateFilter) == _updateFilter;

            if (_isValid && !isSoundSourceWork)
                Terminate();

            UpdatePosition();
        }

        public void Stop(bool immediately = false)
        {
            if((_state | _stopFilter) == _stopFilter) return;

            if (_state == SoundSourceState.Stop)
            {
                if (_isValid)
                {
                    _sequence?.Kill();
                    Terminate();
                }
                
                return;
            }

            _sequence?.Kill();

            _state = SoundSourceState.Stop;
            
            if (_endEffect.IsValid)
            {
                _sequence = DOTween.Sequence();
                _sequence = _endEffect.ApplyEffect(_sequence, _source, 0f).AppendCallback(Terminate);
                _sequence.SetUpdate(_isIgnoreListenerPause);
            }
            else
                Terminate();
        }

        private void Terminate()
        {
            _isValid = false;
            _isPauseWhenTransition = false;
            
            _source.Stop();
            _state = SoundSourceState.Empty;
            
            OnCompleted?.Invoke(this);
        }

        public void Reset()
        {
            _sequence?.Kill();
            
            _isValid = false;
            _state = SoundSourceState.Empty;
            _source.loop = false;
            _source.ignoreListenerPause = false;

            if(_isHaveTargetPoint)
                _transform.localPosition = Vector3.zero;
  
            _isHaveTargetPoint = false;
            _point = null;
            
            _source.Stop();
            _startEffect = _defaultEffect;
            _endEffect = _defaultEffect;
            _source.volume = _data.Settings.Volume;
            _isIgnoreListenerPause = false;
            OnCompleted = null;
            _source.enabled = false;
        }

        private void UpdatePosition()
        {
            if(!_isHaveTargetPoint) return;

            if (!_point.IsAlive)
            {
                _isHaveTargetPoint = false;
                _point = null;
                _transform.localPosition = Vector3.zero;
                return;
            }

            _transform.position = _point.Target.position;
        }
        
        public ISoundSourcePlayer SetLoop(bool isLoop)
        {
            _source.loop = isLoop;
            return this;
        }

        public ISoundSourcePlayer SetIgnoreListenerPause(bool isIgnore)
        {
            _source.ignoreListenerPause = isIgnore;
            _isIgnoreListenerPause = isIgnore;
            return this;
        }

        public ISoundSourcePlayer WithStartSmooth(SoundSmoothEffect effect)
        {
            _startEffect = effect;
            return this;
        }

        public ISoundSourcePlayer WithStopSmooth(SoundSmoothEffect effect)
        {
            _endEffect = effect;
            return this;
        }

        public ISoundSourcePlayer FollowAt(I3DSoundPoint target)
        {
            _point = target;
            _isHaveTargetPoint = true;
            return this;
        }

        public ISoundSourcePlayer SetPosition(Vector3 position)
        {
            _transform.position = position;
            return this;
        }
        
        public ISoundSourcePlayer SetMixerGroup(AudioMixerGroup mixerGroup)
        {
            _source.outputAudioMixerGroup = mixerGroup;
            return this;
        }

        [Flags]
        private enum SoundSourceState
        {
            Empty = 0,
            Ready = 1,
            Play = 2,
            Pause = 4,
            Stop = 8,
            Transition = 16
        }
    }
}