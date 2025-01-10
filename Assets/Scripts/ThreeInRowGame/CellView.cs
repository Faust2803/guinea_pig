using System;
using DG.Tweening;
using Game;
using UnityEngine;

namespace ThreeInRowGame
{
    public class CellView : BaseGameView
    {
        [SerializeField] private GameObject _element;
        [SerializeField] private GameObject _delete;
        [SerializeField] private Animator _deleteEffect;
        [SerializeField] private float _moveSpeed = 1.5F;
        
        private CellMediator _mediator;
        private Tweener _tweener;
        
        
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

        public void Move(GreedElementView element)
        {
            _tweener.Kill();
            _tweener = transform.DOMove(element.transform.position, _moveSpeed).SetEase( Ease.OutExpo ).OnComplete(OnMoveCompleted);
        }

        private void OnMoveCompleted()
        {
            _tweener.Kill();
            Debug.Log("MOVE COMPLETE");
        }
        
    }
}