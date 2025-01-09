using Util;
using Zenject;


namespace Managers.SceneManagers
{
    public class ThreeInRowGameSceneManager : BaseSceneManager
    {
        [Inject] protected FactoryBoolet _factoryBoolet;
        
        
        
        
        private void Start()
        {
            _audio.PlaySound(SoundManager.Enums.SoundId.JumperMusic, isLoop: true, false);
            _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.JumperMusic, 0.5f);
            Init();
        }

        private  void Init()
        {
            
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.JumperMusic);
            
        }
        
    }
}
