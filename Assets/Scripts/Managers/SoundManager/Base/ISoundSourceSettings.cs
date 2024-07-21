using UnityEngine;

namespace Managers.SoundManager.Base
{
    public interface ISoundSourceSettings
    {
        float Volume { get; }
        void Setup(AudioSource source, bool is3D = false);
    }
}