using UI.Panels;
using UnityEngine;
using Zenject;

namespace Managers.SceneManagers
{
    public class BootSceneManager : MonoBehaviour
    {
        [Inject] private UiManager _uiManager;

        private void Awake()
        {
            Application.targetFrameRate = 60;
        }

        private void Start()
        {
           _uiManager.OpenPanel(PanelType.BootPanel);
        }
        
        
    }
}
