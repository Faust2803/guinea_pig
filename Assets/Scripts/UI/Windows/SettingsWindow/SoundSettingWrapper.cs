using System;
using Managers.SoundManager;
using Managers.SoundManager.Base;
using Managers.SoundManager.Enums;
using UI.Misc;
using UnityEngine;

namespace UI.Windows.SettingsWindow
{
    public class SoundSettingWrapper : IDisposable
    {
        public ButtonSliderView BtnSlider;

        private ISoundManager _soundManager;
        private SoundSettingViewData _viewData;
        private SoundType _soundType;
            
        public SoundSettingWrapper(SoundManager.SoundVolumeSetting volumeSettings,
                                   ISoundManager manager, SoundSettingViewData viewData, 
                                   ButtonSliderView sliderView)
        {
            _soundManager = manager;
            _viewData = viewData;
            BtnSlider = sliderView;
            _soundType = volumeSettings.SoundType;
                
            Initialize(volumeSettings);
        }

        private void Initialize(SoundManager.SoundVolumeSetting volumeSettings)
        {
            BtnSlider.Initialize(volumeSettings.Value);
            BtnSlider.WithRange(new Vector2(0f, 1f))
                     .WithLeftLabel(_viewData.Name)
                     .WithBodyColor(_viewData.ColorBodyFront, _viewData.ColorBodyBack);

            UpdateRightLabel(BtnSlider);
            BtnSlider.OnChanged += UpdateRightLabel;
        }
            
        private void UpdateRightLabel(ButtonSliderView view)
        {
            var text = string.Empty;
                
            _soundManager.UpdateVolume(_soundType, view.CurrentValue);
            
            var sliderSize = view.Range.y - view.Range.x;
            var minValue = Mathf.Min(view.Range.y, view.Range.x);
            var percentage = Mathf.Abs((view.CurrentValue - minValue) / sliderSize);
            percentage *= 100f;
            
            text = percentage <= 0.5   ? "OFF" : 
                   percentage >= 99.5f ? "ON"  : percentage.ToString("0") + "%";
            
            view.WithRightLabel(text);
        }

        public void Dispose()
        {
            BtnSlider.OnChanged -= UpdateRightLabel;
        }
    }
}