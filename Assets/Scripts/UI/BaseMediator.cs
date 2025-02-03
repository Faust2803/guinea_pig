using System;
using Managers;
using Zenject;


namespace UI
{
    public class BaseMediator
    {
        public  BaseMediator()
        {
            ProjectContext.Instance.Container.Inject(this);
        }

        [Inject] protected UiManager _uiManager;
        
        protected const float MOVE_POSITION = -1500;
        protected const float ANIMATION_DURATION = 0.1F;
        protected float _moveto;
        protected Action _afterCloseCallback;
        protected object _data;
        
    }
}