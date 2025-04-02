
using Managers.SceneManagers;
using UnityEngine;
using Zenject;

namespace UI.Windows.SettingsWindow
{
    public class SettingsWindowMediator : BaseWindowMediator<SettingsWindowView, WindowData>
    {
        [Inject] SceneLoadManagers scenes;
        protected override void ShowStart()
        {
            base.ShowStart();

            Target.SignOutButton.onClick.AddListener(OnSignOutButtonClicked);
        }
        
        protected override void CloseStart()
        {
            base.CloseStart();
            Target.SignOutButton.onClick.RemoveListener(OnSignOutButtonClicked);
        }

        private  void OnSignOutButtonClicked ()
        {
            PlayerPrefs.DeleteAll();
            scenes.LoadScene(Scene.Boot);
            _uiManager.CloseAllPanels();
            CloseSelf();
        }
    }
}