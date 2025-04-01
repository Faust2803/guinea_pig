using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Panels.TopGamePanel
{
    public class TopGamePanelView : BasePanelView
    {
        [SerializeField] private TextMeshProUGUI  _lifes;
        [SerializeField] private TextMeshProUGUI  _boolets;
        [SerializeField] private TextMeshProUGUI  _enemyes;
        [SerializeField] private TextMeshProUGUI  _score;
        [Space]
        [SerializeField] private Button _endButton;
        
        private Tweener _tweener;

        protected override void CreateMediator()
        {
            _mediator = new TopGamePanelMediator();
        }

       public TextMeshProUGUI  Lifes => _lifes;
       public TextMeshProUGUI  Boolets => _boolets;
       public TextMeshProUGUI  Enemyes => _enemyes;
       public TextMeshProUGUI  Score => _score;
       public Button EndButton => _endButton;

    }
}