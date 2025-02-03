using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.BootPanel
{
    public class BottomPanelView : BasePanelView
    {
        [SerializeField] private Button  _playWindowButton;
        [SerializeField] private GameObject  _achievmentsCompleatPointer;
        
        [SerializeField] private Button  _selectPersonagButton;
        [SerializeField] private Button  _settingsButton;
        [SerializeField] private Button  _upgradeButton;
        [SerializeField] private Button  _shopButton;
        
        private Tweener _tweener;

        protected override void CreateMediator()
        {
            _mediator = new BottomPanelMediator();
        }

        public Button PlayWindowButton => _playWindowButton;
        
        public Button SelectPersonagButton => _selectPersonagButton;
        public Button SettingsButton => _settingsButton;
        public Button UpgradeButton => _upgradeButton;
        public Button ShopButton => _shopButton;

        public void AchievmentsCompleatPointer(bool value)
        {
            _achievmentsCompleatPointer.SetActive(value);
            if (value)
            {
                _tweener.Play();
            }
            else
            {
                _tweener.Kill();
            }
        }

        private void Awake()
        {
            var scale = new Vector3(1.2f, 1.2f, 1.2f);
            _tweener.Kill();
            _tweener = _achievmentsCompleatPointer.transform.DOScale(scale, 0.5f).SetEase( Ease.InOutSine ).SetLoops( -1, LoopType.Yoyo );
            
            
        }
        
    }
}