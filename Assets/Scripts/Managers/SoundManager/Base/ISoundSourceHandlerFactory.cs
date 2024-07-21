using UnityEngine;

namespace Managers.SoundManager.Base
{
    public interface ISoundSourceHandlerFactory
    {
        ISoundSourceHandler Create(ISoundData data, Transform parent, bool isExternal, bool is3D);
    }
}