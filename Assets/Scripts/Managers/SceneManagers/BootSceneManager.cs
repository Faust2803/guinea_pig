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
#if UNITY_ANDROID
            Input.gyro.enabled = false; // Отключает гироскоп
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 30;
#endif
            
        }

        private async void Start()
        {
            await _configDataManager.Init();
            await _auth.Init();
            await _databaseManage.Init();
            _playerManager.Init();
            _uiManager.OpenPanel(PanelType.BootPanel, new PanelData());
           
           
        }
        
        
    }
}
