
using UnityEngine;

namespace Game.Character.Player
{
   
    public class LobbyPlayerCharacterView : CharacterView
    {
        
        protected override void CreateMediator()
        {
            _mediator = new LobbyPlayerCharacterMediator();
        }



    }
}