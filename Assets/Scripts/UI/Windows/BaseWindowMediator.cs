using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Windows
{
    public class BaseWindowMediator: BaseMediator
    {
        public BaseWindowView WindowView { get; set; }
        
        private WindowType _windowType;
        
        public WindowType WindowType
        {
            get { return _windowType; }
        }

        public virtual void Mediate(BaseWindowView value)
        {
            WindowView =  value;
            if(WindowView.AnimationPanel)
                _moveto  = WindowView.AnimationPanel.transform.position.y;
        }
        
        public virtual void SetData(object data)
        {
            _data = data;
        }
        
        public void SetType(WindowType windowType)
        {
            _windowType = windowType;
        }
        
        public virtual void Show()
        {
            ShowStart();
            WindowView.ShowStart();
            if(WindowView.CloseButton)
                WindowView.CloseButton.onClick.AddListener(()=>CloseSelf());
        }
        
        public void Close(Action callback = null)
        {
            if (callback!= null)
            {
                _afterCloseCallback = callback;
            }

            if(WindowView.CloseButton)
                WindowView.CloseButton.onClick.RemoveListener(()=>CloseSelf());
            CloseStart();
        }

        protected virtual void ShowStart()
        {
            if (WindowView.AnimationPanel == null || !WindowView.OpenAnimation)
            {
                ShowEnd();
                return;
            }
            
            // WindowView.WindowPanel.transform.position = new Vector3(WindowView.WindowPanel.transform.position.x,
            //     MOVE_POSITION,
            //        WindowView.WindowPanel.transform.position.z);
            //WindowView.WindowPanel.transform.DOMoveY(_moveto, ANIMATION_DURATION).OnComplete(ShowEnd);
            WindowView.AnimationPanel.transform.localScale = Vector3.zero;
            WindowView.AnimationPanel.transform.DOScale(Vector3.one, WindowView.OpenCloseDuration).OnComplete(ShowEnd);
        }
        
        protected virtual void ShowEnd()
        {

        }
        
        protected virtual void CloseStart()
        { 
            if (WindowView.AnimationPanel == null || !WindowView.CloseAnimation)
            {
                CloseFinish();
                return;
            }
            //WindowView.WindowPanel.transform.DOMoveY(MOVE_POSITION, ANIMATION_DURATION).OnComplete(CloseFinish);
            WindowView.AnimationPanel.transform.DOScale(Vector3.zero, WindowView.OpenCloseDuration).OnComplete(CloseFinish);
        }
        
        protected virtual void CloseFinish()
        { 
            WindowView.Close();
            if (_afterCloseCallback!= null)
            {
                _afterCloseCallback.Invoke();
            }
            
        }
        
        protected virtual void CloseSelf(Action callback = null)
        {
            _uiManager.CloseWindow(callback);
        }
    }
    
    public abstract class BaseWindowMediator<T, Z> : BaseWindowMediator where T : BaseWindowView where Z : WindowData
    {
        public T Target
        {
            get { return WindowView as T; }
        }
    
        public Z Data
        {
            get { return _data as Z; }
        }
    }
}