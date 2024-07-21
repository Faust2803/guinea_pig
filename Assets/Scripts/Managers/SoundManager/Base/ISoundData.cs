using Managers.SoundManager.Data;
using Managers.SoundManager.Enums;
using UnityEngine;

namespace Managers.SoundManager.Base
{
    public interface ISoundData
    {
        ISoundIdInfo IdInfo { get; }
        SoundType SoundType { get; }
        AudioClip Clip { get; }
        ISoundSourceSettings Settings { get; }
    }
}