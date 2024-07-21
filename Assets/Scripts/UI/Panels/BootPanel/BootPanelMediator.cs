
using Common.Statistic;
using DataModels.Achievement;
using Managers;
using Managers.Analytics;
using UI.Windows;
using UI.Windows.SimpleDialogWindow;
using UnityEngine;
using Zenject;

namespace UI.Panels.BootPanel
{
    public class BootPanelMediator :BasePanelMediator <BootPanelView, PanelData>
    {
        [Inject] private AdressableLoaderManager _adressableLoaderManager;
        [Inject] private SceneLoadManagers _sceneLoadManagers;
        [Inject] private INetworkManager _networkManager;
        [Inject] PlayerManager _playerManager;
        [Inject] AuthorizationManager _authorizationManager;
        [Inject] private IStatisticManager _statisticManager;
        [Inject] private AnalyticsManager _analyticsManager;
        [Inject] private AchievementManager _achievementManager;
       

       
        protected override void ShowStart()
        {
            base.ShowStart();
            Target.LoadGameButton.gameObject.SetActive(false);
            Target.LoadObject.SetActive(true);
            Target.ProgressText = "0";

            _adressableLoaderManager.OnLoadingProgress += LoadAdressabeProgress;
            _adressableLoaderManager.OnLoadingDataComplite += LoadAdressabeCompleat;
            _adressableLoaderManager.LoadAdressabless(AssetLabel.Main);
            Target.LoadGameButton.onClick.AddListener(GoToLobby);
            
            //_analyticsManager.LoadingScreen();
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

            GoToLobby();

            //Target.LoadGameButton.gameObject.SetActive(true);
            //Target.LoadObject.SetActive(false);
        }
        
        private void LoadAdressabeProgress(float persent, float loaded, float all)
        {
            Debug.Log($"BootPanelMediator LoadAdressabeProgress progress = {persent}");
            Target.ProgressText = $"Loading {persent}% ({loaded}/{all} MB)";
        }

        private async void GoToLobby()
        {
            var authToken = await _authorizationManager.Authorization();
            Debug.Log($"Auth token = {authToken}");
            var authResult = await _networkManager.Authorization(authToken);
            Debug.Log($"Auth Result == [{authResult}]");
            await _playerManager.SetUpAllPlayerData(authResult);
            await _statisticManager.Initialize();

            _analyticsManager.EnterPlayScreen();
            
            var result = await _achievementManager.GetAchievementsList();
            if (result)
            {
                await _achievementManager.CheckAchievementTarget(new AchievementTarget(TargetType.PlayGame, 1));
            }
            
            
            CloseSelf();
            _sceneLoadManagers.LoadScene(Scene.Lobby);
            
        }

        
        
    }
}