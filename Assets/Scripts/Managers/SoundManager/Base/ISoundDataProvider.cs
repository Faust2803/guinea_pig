using Managers.SoundManager.Enums;

namespace Managers.SoundManager.Base
{
    public interface ISoundDataProvider
    {
        bool TryGetData(SoundId id, out ISoundData data);
        bool TryGetData(string id, out ISoundData data);
    }
}