using Managers.HamsterPreviewManager;
using Managers;
using Zenject;

namespace UI.Windows.AttentionWindow
{
    public class AttentionWindowMediator : BaseWindowMediator<AttentionWindowView, AttentionWindowData>
    {
        [Inject] SceneLoadManagers scenes;
        [Inject] HamsterPreviewManager _hamsterPreviewManager;
        [Inject] UiManager ui;

        public override void SetData(object data)
        {
            base.SetData(data);

            Target.Home.onClick.AddListener(HomeClicked);
            Target.Repeat.onClick.AddListener(RestartClicked);
            _hamsterPreviewManager.ShowPreview(true);
        }

        private void HomeClicked ()
        {
            _hamsterPreviewManager.ShowPreview(false);
            scenes.LoadScene(Scene.Lobby);

            ui.CloseAllWindows();
        }

        private void RestartClicked ()
        {
            _hamsterPreviewManager.ShowPreview(false);
            scenes.ReloadScene();

            ui.CloseAllWindows();
        }
    }
}