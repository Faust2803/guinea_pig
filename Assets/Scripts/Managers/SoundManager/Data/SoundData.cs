using System;
using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UnityEngine;

namespace Managers.SoundManager.Data
{
    [Serializable]
    public class SoundData : ISoundData
    {
#if UNITY_EDITOR
        //This variable dont use in SoundManager system. Only for best view into inspector
        public string EditorLabel;
#endif
        public ISoundIdInfo IdInfo => _idData;
        public SoundType SoundType => _soundType;
        public AudioClip Clip => _clip[UnityEngine.Random.Range(0, _clip.Length)];
        public ISoundSourceSettings Settings => _settings;
        
        [SerializeField] private AudioClip[] _clip;
        [SerializeField] private SoundKeyPairId _idData;
        [SerializeField] private SoundType _soundType;
        [SerializeField] private SoundSourceSetting _settings;

        public void SetKeyInfo(SoundKeyPairId data)
        {
            _idData = data;
        }
    }
}