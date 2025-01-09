using Game;
using UnityEngine;

namespace ThreeInRowGame
{
    public class CellView : BaseGameView
    {
        [SerializeField] private GameObject _element;
        [SerializeField] private GameObject _delete;
        [SerializeField] private Animator _deleteEffect;
        
        private CellMediator _mediator;
        
        public GameObject Element => _element;
        public GameObject Delete => _delete;
        public Animator DeleteEffect => _deleteEffect;
        
        public CellMediator Mediator => _mediator;
        
        public override void Init()
        {
            base.Init();
            _mediator.Mediate(this);
        }

        protected override void CreateMediator()
        {
            _mediator = new CellMediator();
        }
    }
}