using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

namespace Game.Character
{
    public abstract class CharacterView : BaseGameView
    {
        
        [SerializeField] private NavMeshAgent _navMeshAgent;
        [SerializeField] protected Transform _weaponAttachment;
        [SerializeField] protected Animator _animator ;
        [SerializeField] protected LayerMask _layerMask ;
        
        protected CharacterMediator _mediator;

       
        
        public CharacterMediator Mediator => _mediator;
        public Transform WeaponAttachment => _weaponAttachment;
        public Animator Animator => _animator;
        public LayerMask LayerMask => _layerMask;
        

        public void OnCreateMediator(out CharacterMediator mediator)
        {
            mediator = _mediator;
        }

        public override void Init()
        {
            base.Init();
            _mediator.Mediate(this);
        }
        
        private void Update()
        {
            _mediator.GameLifeСycle();
        }
        
        public NavMeshAgent NavMeshAgent => _navMeshAgent;

    }
}