using System;
using DG.Tweening;
using UnityEngine;

namespace UI.Panels
{
    public class BasePanelMediator: BaseMediator
    {
        
        
        public BasePanelView PanelView { get; set; }
        
        private PanelType _panelType;
        
        public PanelType PanelType => _panelType;
        public bool DeleteAfterClose => PanelView.DeleteAfterClose;

        public virtual void Mediate(BasePanelView value)
        {
            PanelView =  value;
            _moveto  = PanelView.Panel.transform.position.y;
        }
        
        public virtual void SetData(object data)
        {
            _data = data;
        }
        
        public void SetType(PanelType windowType)
        {
            _panelType = windowType;
        }

        public virtual void Show()
        {
            ShowStart();
            PanelView.ShowStart();
        }
        
        public void Close(Action callback = null)
        {
            if (callback!= null)
            {
                _afterCloseCallback = callback;
            }
            CloseStart();
        }
        
        protected virtual void ShowStart()
        {
            if (PanelView.Panel == null || !PanelView.OpenAnimation)
            {
                ShowEnd();
                return;
            }
            
            PanelView.Panel.transform.position = new Vector3(PanelView.Panel.transform.position.x,
                MOVE_POSITION,
                   PanelView.Panel.transform.position.z);
            PanelView.Panel.transform.DOMoveY(_moveto, ANIMATION_DURATION).OnComplete(ShowEnd);
        }
        
        protected virtual void ShowEnd()
        {

        }
        
        protected virtual void CloseStart()
        { 
            if (PanelView.Panel == null || !PanelView.CloseAnimation)
            {
                CloseFinish();
                return;
            }
            PanelView.Panel.transform.DOMoveY(MOVE_POSITION, ANIMATION_DURATION).OnComplete(CloseFinish);
        }
        
        protected virtual void CloseFinish()
        { 
            PanelView.Close();
            if (_afterCloseCallback!= null)
            {
                _afterCloseCallback.Invoke();
            }
        }

        protected virtual void CloseSelf()
        {
            _uiManager.ClosePanel(_panelType);
        }
    }
    
    public abstract class BasePanelMediator<T, Z> : BasePanelMediator where T : BasePanelView where Z : PanelData
    {
        public T Target
        {
            get { return PanelView as T; }
        }
    
        public Z Data
        {
            get { return _data as Z; }
        }
    }
}