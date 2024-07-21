using UI.Misc;
using UI.Windows.SettingsWindow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.PauseWindow
{
    public class PauseWindowView : BaseWindowView
    {
        public Button BtnContinue;
        public Button BtnToLobby;
        public RectTransform ParentForSettings;
        public ButtonSliderView BtnSliderPrefab;
        public SoundSettingViewConfig Config;
        
        protected override void CreateMediator()
        {
            _mediator = new PauseWindowMediator();
        }
    }
}