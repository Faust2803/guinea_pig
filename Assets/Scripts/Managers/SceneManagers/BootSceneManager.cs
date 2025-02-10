using Auth;
using Managers.ConfigDataManager;
using Managers.DatabaseManager;
using UI.Panels;
using UnityEngine;
using Zenject;

namespace Managers.SceneManagers
{
    public class BootSceneManager : MonoBehaviour
    {
        [Inject] private UiManager _uiManager;
        [Inject] private IConfigDataManager _configDataManager;
        [Inject] private IAuth _auth;
        [Inject] private IDatabaseManager _databaseManage;
        [Inject] private PlayerManager _playerManager;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private async void Start()
        {
            await _configDataManager.Init();
            await _auth.Init();
            await _databaseManage.Init();
            _playerManager.Init();
            _uiManager.OpenPanel(PanelType.BootPanel);
           
           
        }
        
        
    }
}
