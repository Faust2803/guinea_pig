using System;
using Managers.SoundManager.Enums;
using UnityEngine;

namespace Managers.SoundManager.Data
{
    [Serializable]
    public class SoundKeyPairId : ISoundIdInfo
    {
        public string Key => _key;
        public SoundId Id => _id;

        [SerializeField] private string _key;
        [SerializeField] private SoundId _id;

        public SoundKeyPairId(string key, SoundId id)
        {
            _key = key;
            _id = id;
        }
    }
}