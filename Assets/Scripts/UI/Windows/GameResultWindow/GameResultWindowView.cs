using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;


namespace UI.Windows.GameResultWindow
{
    public class GameResultWindowView : BaseWindowView
    {
        [SerializeField] private TextMeshProUGUI _isWin;
        [SerializeField] private TextMeshProUGUI _bullets;
        [SerializeField] private TextMeshProUGUI _enemyes;
        [SerializeField] private TextMeshProUGUI _lives;
        [SerializeField] private TextMeshProUGUI _score;
        [Space]
        [SerializeField] private Button _lobbyButton;
        [SerializeField] private Button _repeatButton;
        public Button LobbyButton => _lobbyButton;
        public Button RepeatButton => _repeatButton;
        public TextMeshProUGUI IsWin => _isWin;
        public TextMeshProUGUI Bullets => _bullets;
        public TextMeshProUGUI Enemyes => _enemyes;
        public TextMeshProUGUI Lives => _lives;
        public TextMeshProUGUI Score => _score;
        protected override void CreateMediator()
        {
            _mediator = new GameResultWindowMediator();
        }
        
        
        
    }
}