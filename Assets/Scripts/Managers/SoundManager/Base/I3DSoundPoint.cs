using UnityEngine;

namespace Managers.SoundManager.Base
{
    public interface I3DSoundPoint
    {
        Transform Target { get; }
        bool IsAlive { get; }
    }
}