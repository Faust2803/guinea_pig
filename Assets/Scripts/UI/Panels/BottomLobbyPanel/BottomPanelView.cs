using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.BootPanel
{
    public class BottomPanelView : BasePanelView
    {
        [SerializeField] private Button  _collectionWindowButton;
        [SerializeField] private Button  _marketWindowButton;
        [SerializeField] private Button  _newsWindowButton;
        [SerializeField] private Button  _playWindowButton;
        [SerializeField] private Button  _profileWindowButton;
        [SerializeField] private Button  _walletWindowButton;
        [SerializeField] private Button  _settingsWindowButton;
        [SerializeField] private Button  _tamagochWindowButton;
        [SerializeField] private Button  _leaderboardWindowButton;
        [SerializeField] private Button  _piramidWindowButton;
        [SerializeField] private Button  _achievmentsWindowButton;
        
        [SerializeField] private GameObject  _achievmentsCompleatPointer;

        [SerializeField] private Button  _forTestingButton;
        [SerializeField] private Button  _resetProgressButton;
        [SerializeField] private Button _hamsterOfMoonButton;
        
        private Tweener _tweener;

        protected override void CreateMediator()
        {
            _mediator = new BottomPanelMediator();
        }
        

        public Button CollectionWindowButton => _collectionWindowButton;
        public Button MarketWindowButton => _marketWindowButton;
        public Button NewsWindowButton => _newsWindowButton;
        public Button PlayWindowButton => _playWindowButton;
        public Button ProfileWindowButton => _profileWindowButton;
        public Button WalletWindowButton => _walletWindowButton;
        public Button SettingsWindowButton => _settingsWindowButton;
        public Button ForTestingButton => _forTestingButton;
        public Button ResetProgressButton => _resetProgressButton;
        public Button TamagochWindowButton => _tamagochWindowButton;
        public Button LeaderboardWindowButton => _leaderboardWindowButton;
        public Button PiramidWindowButton => _piramidWindowButton;
        public Button AchievmentsWindowButton => _achievmentsWindowButton;
        public Button HamsterOfMoonButton => _hamsterOfMoonButton;

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