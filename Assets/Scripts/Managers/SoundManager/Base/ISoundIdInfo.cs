using Managers.SoundManager.Enums;

namespace Managers.SoundManager.Data
{
    public interface ISoundIdInfo
    {
        string Key { get; }
        SoundId Id { get; }
    }
}