using ThreeInRowGame;
using Util;
using Zenject;


namespace Managers.SceneManagers
{
    public class ThreeInRowGameSceneManager : BaseSceneManager
    {
        [Inject] protected FactoryLevel _factoryLevel;
        
        
        
        
        private void Start()
        {
            _loadingObject.SetActive(true);
            // _audio.PlaySound(SoundManager.Enums.SoundId.JumperMusic, isLoop: true, false);
            // _audio.UpdateVolumeSound(SoundManager.Enums.SoundId.JumperMusic, 0.5f);
            Init();
        }

        private void Init()
        {
             CreateLevel(0);
             
             _loadingObject.SetActive(false);
        }

        private LevelView CreateLevel(int level)
        {
            var view = _factoryLevel.Create(level);
            view.gameObject.transform.SetParent(_gameArea,false);
            return view;
        }

        private void OnDestroy()
        {
            _audio.StopSound(SoundManager.Enums.SoundId.JumperMusic);
            
        }
        
        
        
    }
}
