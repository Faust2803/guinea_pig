using TMPro;
using UI.Misc;
using UI.Windows.SettingsWindow;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.ProfileWindow
{
    public class ProfileWindowView : BaseWindowView
    {
        [Header("Profile refereces")]
        public StatisticItemView ItemViewPrefab;
        public RectTransform ParentForItems;
        public Sprite BeamSprite;
        public TMP_Text PlayerNameText;

        [Header("Profile change name")]
        public TMP_InputField InputField;
        public Button ChangeNicknameButton;

        [Header("Setting references")]
        public ButtonSliderView BtnSliderPrefab;        
        public SoundSettingViewConfig Config;
        public RectTransform ParentForBtns;


        protected override void CreateMediator()
        {
            _mediator = new ProfileWindowMediator();
        }
    }
}