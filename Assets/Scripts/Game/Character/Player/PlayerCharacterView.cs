
namespace Game.Character.Player
{
    public class PlayerCharacterView : CharacterView
    {
        protected override void CreateMediator()
        {
            _mediator = new PlayerCharacterMediator();
        }
        
    }
}