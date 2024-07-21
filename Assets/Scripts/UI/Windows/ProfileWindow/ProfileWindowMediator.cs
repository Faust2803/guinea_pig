
using System.Collections.Generic;
using Common.Statistic;
using Managers;
using Managers.SoundManager.Base;
using TMPro;
using UI.Windows.SettingsWindow;
using UI.Windows.SimpleDialogWindow;
using UnityEngine;
using Zenject;

namespace UI.Windows.ProfileWindow
{
    public class ProfileWindowMediator : BaseWindowMediator<ProfileWindowView, WindowData>
    {
        [Inject] private IStatisticManager _statisticManager;
        [Inject] private PlayerManager _playerManager;
        [Inject] private ISoundManager _soundManager;

        private List<GameObject> _items = new List<GameObject>();
        private List<SoundSettingWrapper> _wrappers = new List<SoundSettingWrapper>();

        public override void Show()
        {
            base.Show();

            _uiManager.ClosePanel(Panels.PanelType.TamagochiPanel);

            ((TextMeshProUGUI)Target.InputField.placeholder).SetText(_playerManager.PlayerName);
            Target.ChangeNicknameButton.onClick.AddListener(async () =>
            {
                if (string.IsNullOrEmpty(Target.InputField.text))
                {
                    var windowData = new SimpleDialogWindowData
                    {
                        Title = "Error",
                        Description = "Please enter valid name",
                        NoButtonActive = false,
                    };
                    _uiManager.ForceOpenWindow(WindowType.SimpleDialogWindow, windowData);
                }
                else
                {
                    var response = await _playerManager.RenameUser(Target.InputField.text);
                    if (response)
                    {
                        var windowData = new SimpleDialogWindowData
                        {
                            Title = "Awesome",
                            Description = "You have changed your nickname succsesfully!",
                            NoButtonActive = false,
                        };
                        _uiManager.ForceOpenWindow(WindowType.SimpleDialogWindow, windowData);
                    }
                    else
                    {
                        var windowData = new SimpleDialogWindowData
                        {
                            Title = "Error",
                            Description = "User nickname already exist. Please, enter new one.",
                            NoButtonActive = false,
                        };
                        _uiManager.ForceOpenWindow(WindowType.SimpleDialogWindow, windowData);
                    }
                }
            });

            var statisticInfo =  _statisticManager.GetStatisticInfo();

            Target.PlayerNameText.text = _playerManager.PlayerName;

            foreach (var info in statisticInfo)
            {
                var item = Object.Instantiate(Target.ItemViewPrefab, Target.ParentForItems);
                _items.Add(item.gameObject);
                SetupItem(item, info);
            }
            
            var soundData = _soundManager.GetVolumeSettings();

            foreach (var data in soundData)
            {
                var viewData = Target.Config.GetViewData(data.SoundType);

                var slider = Object.Instantiate(Target.BtnSliderPrefab, Target.ParentForBtns);
                var wrapper = new SoundSettingWrapper(data, _soundManager, viewData, slider);
                _wrappers.Add(wrapper);
            }
        }

        private void SetupItem(StatisticItemView item, IStatisticUnitInfo info)
        {
            if (info.Id != StatisticSourceId.Beans)
            {
                var name = info.Id == StatisticSourceId.PlayGame ? "Games" : "None";
                item.Init(name, info.TotalAmount);
            }
            else
                item.Init("Beans", info.TotalAmount, Target.BeamSprite);

        }

        protected override void CloseFinish()
        {
            base.CloseFinish();

            _uiManager.OpenPanel(Panels.PanelType.TamagochiPanel);

            foreach (var item in _items)
                Object.Destroy(item);
            
            foreach (var wrapper in _wrappers)
            {
                wrapper.Dispose();
                Object.Destroy(wrapper.BtnSlider.gameObject);
            }
            
            _wrappers.Clear();
        }
    }
}