using UnityEngine;

namespace Game.Character
{
    public abstract class CharacterView : BaseGameView
    {
        protected CharacterMediator _mediator;
        
        public CharacterMediator Mediator => _mediator;

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