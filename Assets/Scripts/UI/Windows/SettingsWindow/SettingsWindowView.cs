using UI.Misc;
using UnityEngine;

namespace UI.Windows.SettingsWindow
{
    public class SettingsWindowView : BaseWindowView
    {
        public ButtonSliderView BtnSliderPrefab;
        public RectTransform ParentForSliders;
        public SoundSettingViewConfig Config;
        
        protected override void CreateMediator()
        {
            _mediator = new SettingsWindowMediator();
        }
    }
}