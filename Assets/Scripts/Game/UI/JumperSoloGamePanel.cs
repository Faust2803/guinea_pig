using Managers;
using System;
using TMPro;
using UI.Panels;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using DG.Tweening;

namespace Game.Jumper
{
    public class JumperSoloGamePanel : BasePanelView
    {
        public VirtualAxisUI joystick;

        [Space]
        public AbilityFillingUI peanAbility;
        [Space]
        public AbilityFillingUI cornAbility;
        [Space]       
        public AbilityFillingUI seedAbility;

        [SerializeField] DynamicSizeHealthBar healthBar;
        [SerializeField] JumperSoloHeightBar hbar;
        [SerializeField] GameScoreCounterUI gameScore;
        [SerializeField] Button backButton;
        [Space]
        [SerializeField] private GameObject _peanButtonTutorial;
        [SerializeField] private GameObject _peanStartTutorial;
        [SerializeField] private GameObject _toTheRightTutorial;
        [SerializeField] private GameObject _toTheLeftTutorial;


        private GameLayerController jumperGame;

        public bool PlayerControllsVisibled
        {
            set
            {
                //joystick?.gameObject.SetActive(value);
            }
        }
        
        public Button BackButton => backButton;
        
        
        public GameObject CornButtonContent => cornAbility.buttonContent;
        public GameObject SeedButtonContent => seedAbility.buttonContent;
        
        public Button CornButton => cornAbility.buttonComponent;
        public Button PeanButton => peanAbility.buttonComponent;
        public Button SeedButton => seedAbility.buttonComponent;
        
        public GameObject PeanButtonTutorial => _peanButtonTutorial;
        public GameObject ToTheRightTutorial => _toTheRightTutorial;
        public GameObject ToTheLeftTutorial => _toTheLeftTutorial;
        public GameObject PeanStartTutorial => _peanStartTutorial;
        public JumperSoloPlayer JumperSoloPlayer;

        public int ShpereId { get; private set; }

        public void SetupGameController(GameLayerController game)
        {
            jumperGame = game;
            jumperGame.game.IsGameEnded.OnValueChanged += GameEnded;
            var hs = game.GetHeightscoreFromStartData();
            hbar.SetHeightscoreLevel(hs.Item1, hs.Item2);
            JumperSoloPlayer = game.Player.GetComponent<JumperSoloPlayer>();
            gameScore.SetupTarget(JumperSoloPlayer);
            healthBar.LinkWithPlayer(JumperSoloPlayer);
        }

        private void GameEnded (bool old, bool current)
        {
            if(old == false && current)
            {
                jumperGame.game.IsGameEnded.OnValueChanged -= GameEnded;
                healthBar.IsVisibled = false;
            }
        }

        public void LeaveGame()
        {
            //scenes.LoadScene(Scene.Lobby);
        }

        private void Update()
        {
            if (jumperGame != null)
            {
                ShpereId = jumperGame.GetPlayerShpereId();
                hbar.SetPlayerPos(ShpereId, jumperGame.GetPercentOfSphereByObject(true));
                hbar.SetBank(jumperGame.game.StartData.moon_bank);
                hbar.SetSmogLevel(jumperGame.GetSmogShpereId(), jumperGame.GetPercentOfSphereByObject(false));
            }
        }

        protected override void CreateMediator()
        {
            _mediator = new JumperGamePanelMediator();
        }
    }

    [Serializable]
    public class AbilityUIBase
    {
        public GameObject buttonContent;
        public Button buttonComponent;
        public TextMeshProUGUI textComponent;
    }

    [Serializable]
    public class AbilityFillingUI : AbilityUIBase
    {
        public GameObject emptyGameObject;
        public GameObject halfGameObject;
        public GameObject readyGameObject;

        private Tween _currentTween = null;

        public void ChangeState(AbilityFillingState state)
        {
            ResetState();
            switch (state)
            {
                case AbilityFillingState.Empty:
                    emptyGameObject.SetActive(true);
                    break;
                case AbilityFillingState.Partial:
                    halfGameObject.SetActive(true);
                    break;
                case AbilityFillingState.Full:
                    readyGameObject.SetActive(true);
                    AnimateReadyState();
                    break;
                default:
                    emptyGameObject.SetActive(true);
                    break;
            }
            AnimateButton();
        }

        private void ResetState()
        {
            emptyGameObject.SetActive(false);
            halfGameObject.SetActive(false);
            readyGameObject.SetActive(false);
        }

        private void AnimateReadyState()
        {
            buttonContent.transform.DOScale(Vector3.one * 1.25f, 0.3f).SetEase(Ease.OutCubic).OnComplete(() =>
            {
                buttonContent.transform.DOScale(Vector3.one, 0.25f).SetEase(Ease.OutCubic);
            });
        }

        private void AnimateButton()
        {
            //emptyGameObject.transform.parent.gameObject.DoScaleUpAndReset();
        }
    }

    [Serializable]
    public class VirtualAxisUI
    {
        public GameControllButton Positive;
        public GameControllButton Negative;

        public float Axis =>
            Positive.Axis != 0 ? Positive.Axis : 
            (Negative.Axis != 0 ? Negative.Axis : 0);
    }
}