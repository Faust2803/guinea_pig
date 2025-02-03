using System;
using System.Collections.Generic;
using DG.Tweening;
using Game;
using UnityEngine;
using Vector2 = System.Numerics.Vector2;

namespace ThreeInRowGame
{
    public class CellView : BaseGameView
    {
        [SerializeField] private GameObject _element;
        [SerializeField] private GameObject _delete;
        [SerializeField] private Animator _deleteEffect;
        [SerializeField] private float _moveSpeed = 1.5F;
        [SerializeField] private int _deleteTimeout = 1;
        
        private CellMediator _mediator;
        private Tweener _tweener;
        //private Sequence _sequence = DOTween.Sequence();
        public event Action MoveCompletedEvent;
        
        
        public GameObject Element => _element;
        public GameObject Delete => _delete;
        public Animator DeleteEffect => _deleteEffect;
        public int DeleteTimeout => _deleteTimeout;
        
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

        public void Move(Stack<Vector3> coordinateStack, bool isSwapping = false)
        {
            if (isSwapping)
            {
                _tweener.Kill();
                _tweener = transform.DOMove(coordinateStack.Pop(), 0.15F).OnComplete(OnMoveCompleted);
            }
            else
            {
                var _sequence = DOTween.Sequence();
                while (coordinateStack.Count > 0)
                {
                    _sequence.Append(transform.DOMove(coordinateStack.Pop(), 0.2F));
                }

                //_sequence.SetEase(Ease.OutExpo);
                _sequence.OnComplete(OnMoveCompleted);
                _sequence.Play();
            }
        }

        private void OnMoveCompleted()
        {
            _tweener.Kill();
            //Debug.Log("MOVE COMPLETE");
            MoveCompletedEvent?.Invoke();
        }
        
    }
}