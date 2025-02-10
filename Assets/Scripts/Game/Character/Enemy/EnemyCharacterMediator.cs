
namespace Game.Character.Enemy
{
    public class EnemyCharacterMediator : CharacterMediator<EnemyCharacterView, CharacterData>
    {
        
        
        public override void SetData(object data)
        {
            base.SetData(data);
            Target.transform.position = Data.transform.position;        
        }
    }
}