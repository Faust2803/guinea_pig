
using UnityEngine;

namespace Game.Character.Enemy
{
    public class EnemyCharacterView : CharacterView
    {
        protected override void CreateMediator()
        {
            _mediator = new EnemyCharacterMediator();
        }
    }
}