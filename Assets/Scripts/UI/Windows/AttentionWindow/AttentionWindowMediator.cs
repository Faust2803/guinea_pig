using Managers;
using Managers.SceneManagers;
using Zenject;

namespace UI.Windows.AttentionWindow
{
    public class AttentionWindowMediator : BaseWindowMediator<AttentionWindowView, AttentionWindowData>
    {
        [Inject] SceneLoadManagers scenes;
        [Inject] UiManager ui;

        public override void SetData(object data)
        {
            base.SetData(data);

            Target.Home.onClick.AddListener(HomeClicked);
            Target.Repeat.onClick.AddListener(RestartClicked);
        }

        private void HomeClicked ()
        {
            scenes.LoadScene(Scene.Lobby);

            ui.CloseAllWindows();
        }

        private void RestartClicked ()
        {
            scenes.ReloadScene();

            ui.CloseAllWindows();
        }
    }
}