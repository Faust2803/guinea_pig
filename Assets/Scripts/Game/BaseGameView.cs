using UnityEngine;

namespace Game
{
    public abstract class BaseGameView : MonoBehaviour
    {
        public virtual void Init()
        {
            CreateMediator();
        }
        
        protected abstract void CreateMediator();
    }
}