using System.Collections.Generic;
using Managers.SoundManager.Base;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.Windows.SettingsWindow
{
    public class SettingsWindowMediator : BaseWindowMediator<SettingsWindowView, SettingsWindowData>
    {
        [Inject] private ISoundManager _soundManager;

        private List<SoundSettingWrapper> _wrappers = new List<SoundSettingWrapper>();
        
        public override void Show()
        {
            base.Show();
            var soundData = _soundManager.GetVolumeSettings();

            foreach (var data in soundData)
            {
                var viewData = Target.Config.GetViewData(data.SoundType);

                var slider = Object.Instantiate(Target.BtnSliderPrefab, Target.ParentForSliders);
                var wrapper = new SoundSettingWrapper(data, _soundManager, viewData, slider);
                _wrappers.Add(wrapper);
            }
        }

        protected override void CloseFinish()
        {
            base.CloseFinish();
            foreach (var wrapper in _wrappers)
            {
                wrapper.Dispose();
                Object.Destroy(wrapper.BtnSlider.gameObject);
            }
        }
    }

    public class SettingsWindowData : WindowData
    {
    }
}