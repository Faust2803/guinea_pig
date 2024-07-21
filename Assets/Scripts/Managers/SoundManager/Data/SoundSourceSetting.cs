using System;
using Managers.SoundManager.Base;
using UnityEngine;

namespace Managers.SoundManager.Data
{
    [Serializable]
    public class SoundSourceSetting : ISoundSourceSettings
    {
        public float Volume => _volume;
        
        [SerializeField, Range(0f, 1f)] private float _volume = 1f;
        [SerializeField, Range(-3f, 3f)] private float _pitch = 1f;
        [SerializeField] private bool _is3D = false;
        [SerializeField, Min(1)] private float _minDistance = 1f;
        [SerializeField, Min(10)] private float _maxDistance = 500f;

        public void Setup(AudioSource source, bool is3D = false)
        {
            source.volume = _volume;
            source.pitch = _pitch;
            source.spatialBlend = _is3D && is3D ? 1f : 0f;
            source.spread = _is3D && is3D ? -360f : 0f;
            source.minDistance = _minDistance;
            source.maxDistance = _maxDistance;
        }
    }
}