using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Character
{
    public abstract class CharacterView : BaseGameView
    {
        
        [SerializeField] protected Transform _weaponAttachment;
        [SerializeField] protected float _moveSpeed = 10f;
        
        protected CharacterMediator _mediator;
        protected Vector3 _moveDirection;
        protected float _velocity;

        protected const float GRAVITY = -9.81F;
        
        
        public CharacterMediator Mediator => _mediator;
        public Transform WeaponAttachment => _weaponAttachment;
        public float MoveSpeed => _moveSpeed;
        public Vector3 MoveDirection => _moveDirection;
        

        public void OnCreateMediator(out CharacterMediator mediator)
        {
            mediator = _mediator;
        }

        public override void Init()
        {
            base.Init();
            _mediator.Mediate(this);
        }

    }
}