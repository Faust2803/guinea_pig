using TMPro;
using UI.Windows;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Game.Jumper
{
    public class GameResultWindowView : BaseWindowView
    {
        [SerializeField] GameScoreCounterUI counter;
        [SerializeField] Button lobbyButton;
        [SerializeField] Button repeatButton;

        [SerializeField] GameObject[] winObjects;
        [SerializeField] GameObject[] looseObjects;
        [SerializeField] private TextMeshProUGUI _subtitle;

        protected override void CreateMediator()
        {
            _mediator = new GameResultWindowMediator();
        }

        public void LinkListeners (UnityAction onLobbyButtonClicked, 
            UnityAction onRepeatButtonClicked)
        {
            if (onLobbyButtonClicked != null)
                lobbyButton.onClick.AddListener(onLobbyButtonClicked);

            if (onRepeatButtonClicked != null)
                repeatButton.onClick.AddListener(onRepeatButtonClicked);
        }

        public void SetupResult (IPlayerBase player, bool isWinner)
        {
            counter.SetupTarget(player);
            SetupView(isWinner);
        }

        private void SetupView (bool isWin)
        {
            for (var x = 0; x < winObjects.Length; x++)
                winObjects[x]?.SetActive(isWin);

            for (var x = 0; x < looseObjects.Length; x++)
                looseObjects[x]?.SetActive(!isWin);
        }
        
        public TextMeshProUGUI Subtitle => _subtitle;
    }
}