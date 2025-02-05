using Managers;
using Managers.SceneManagers;
using UnityEngine;
using Zenject;

namespace UI.Panels.BootPanel
{
    public class BootPanelMediator :BasePanelMediator <BootPanelView, PanelData>
    {
        [Inject] private AdressableLoaderManager _adressableLoaderManager;
        [Inject] private SceneLoadManagers _sceneLoadManagers;

        protected override void ShowStart()
        {
            base.ShowStart();
            Target.LoadGameButton.gameObject.SetActive(false);
            Target.LoadObject.SetActive(true);
            Target.ProgressText = "0";

            _adressableLoaderManager.OnLoadingProgress += LoadAdressabeProgress;
            _adressableLoaderManager.OnLoadingDataComplite += LoadAdressabeCompleat;
            Target.LoadGameButton.onClick.AddListener(GoToLobby);
            _ = _adressableLoaderManager.LoadAdressabless(AssetLabel.Main);
        }
        
        protected override void CloseStart()
        { 
            base.CloseStart();
            Target.LoadGameButton.onClick.RemoveListener(GoToLobby);
        }
        
        private void LoadAdressabeCompleat()
        {
            Debug.Log("BootPanelMediator LoadAdressabeCompleat");
            
            _adressableLoaderManager.OnLoadingDataComplite -= LoadAdressabeCompleat;
            _adressableLoaderManager.OnLoadingProgress -= LoadAdressabeProgress;

            //GoToLobby();

            Target.LoadGameButton.gameObject.SetActive(true);
            Target.LoadObject.SetActive(false);
        }
        
        private void LoadAdressabeProgress(float persent, float loaded, float all)
        {
            Debug.Log($"BootPanelMediator LoadAdressabeProgress progress = {persent}");
            Target.ProgressText = $"Loading {persent}% ({loaded}/{all} MB)";
        }

        private void GoToLobby()
        {
            CloseSelf();
            _sceneLoadManagers.LoadScene(Scene.Lobby);
            
        }
    }
}