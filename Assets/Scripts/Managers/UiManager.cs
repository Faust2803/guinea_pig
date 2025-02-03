using System;
using System.Collections.Generic;
using Installers;
using UI.Panels;
using UI.Windows;
using UnityEngine;
using Util;
using Zenject;

namespace Managers
{
    public class UiManager:IInitializable
    { 
        [Inject] private FactoryWindow _factoryWindow;
        [Inject] private FactoryPanel _factoryPanels;
        [Inject] private WindowsInstaller _windowPull;
        [Inject] private PanelsInstaller _panelPull;

        private BaseWindowMediator _currentWindow;
        private Dictionary<WindowType, BaseWindowMediator> _allWindows;
        private Dictionary<PanelType, BasePanelMediator> _allPanels;
        public BaseWindowMediator CurrentWindow => _currentWindow;
        public BasePanelMediator GetPanelInCreated (PanelType type) => _allPanels.ContainsKey(type) ? _allPanels[type] : null;
        
        private Queue<WindowType> _windowsQueue = new Queue<WindowType>();
        private Queue<object> _windowsDataQueue = new Queue<object>();
        private Stack<BaseWindowMediator> _windowsStack = new Stack<BaseWindowMediator>();
        public void Initialize()
        {
            _allWindows = new Dictionary<WindowType, BaseWindowMediator>();
            _allPanels = new Dictionary<PanelType, BasePanelMediator>();
        }

        public void OpenWindow(WindowType windowType, object data = null)
        {
            if (_currentWindow == null)
            {
               CreateWindow(windowType, data);
            }
            else
            {
                if (!_windowsQueue.Contains(windowType) || (data as WindowData).repeatedInQueue)
                {
                    _windowsQueue.Enqueue(windowType);
                    _windowsDataQueue.Enqueue(data);
                }
            }
        }
        
        public void ForceOpenWindow(WindowType windowType, object data = null)
        {
            if (_currentWindow != null)
            {
                if (_currentWindow.WindowType == windowType)
                {
                    return;
                }
                _windowsStack.Push(_currentWindow);
            }
            CreateWindow(windowType, data);
        }

        private void CreateWindow(WindowType windowType, object data = null)
        {
            if (_allWindows.ContainsKey(windowType))
            {
                _currentWindow = _allWindows[windowType];
            }
            else
            {
                _currentWindow = GetWindow(windowType);
                if (_currentWindow == null) return;
                _allWindows.Add(windowType, _currentWindow);
            }
            _currentWindow.SetData(data);
            _currentWindow.Show();
        }

        public void CloseAllWindows ()
        {
            foreach (var item in _allWindows)
                item.Value.Close();
            _allWindows.Clear();
            _windowsStack.Clear();
            _currentWindow = null;
        }

        public void OpenPanel(PanelType panelType, object data = null)
        {
            BasePanelMediator panel;
            if (_allPanels.ContainsKey(panelType))
            {
                panel = _allPanels[panelType];
            }
            else
            {
                panel = GetPanel(panelType);
                if (panel == null) return;
                _allPanels.Add(panelType, panel);
            }
            panel.SetData(data);
            panel.Show();
        }
        
        public void ForceOpenPanel(PanelType panelType, object data = null)
        {
            OpenPanel(panelType, data);
        }
        
        public void CloseWindow(Action callback = null)
        {
            _currentWindow.Close(callback);
            if (_currentWindow.DeleteAfterClose)
            {
                _allWindows.Remove(_currentWindow.WindowType);
            }

            _currentWindow = null;

            if (_windowsStack.Count > 0)
            {
                _currentWindow = _windowsStack.Pop();
            }
            else
            {
                if (_windowsQueue.Count > 0)
                {
                    OpenWindow(_windowsQueue.Dequeue(), _windowsDataQueue.Dequeue());
                }
            }
        }
        
        public void CloseAllPanels(Action callback = null)
        {
            foreach (var t in _allPanels)
            {
                t.Value.Close();
            }
            _allPanels.Clear();
        }
        
        public void ClosePanel(PanelType panelType, Action callback = null)
        {
            if (!_allPanels.ContainsKey(panelType)) return;
            var panel = _allPanels[panelType];
            panel.Close();
            if (panel.DeleteAfterClose)
            {
                _allPanels.Remove(panelType);
            }
        }

        private BaseWindowMediator GetWindow(WindowType windowType)
        {
            var view = LoadPrefab(windowType);
            if (view == null) return null;
        
            view.Init();
            BaseWindowMediator mediator;
            view.OnCreateMediator(out mediator);
            mediator.SetType(windowType);
            view.gameObject.SetActive(false);
        
            return mediator;
        }
        
        private BasePanelMediator GetPanel(PanelType panelType)
        {
            var view = LoadPanelPrefab(panelType);
            if (view == null) return null;
        
            view.Init();
            BasePanelMediator mediator;
            view.OnCreateMediator(out mediator);
            mediator.SetType(panelType);
            view.gameObject.SetActive(false);
        
            return mediator;
        }
        
        private BaseWindowView LoadPrefab(WindowType windowType)
        {
            var view = _factoryWindow.Create(windowType);
            view.gameObject.transform.SetParent(_windowPull.transform,false);
            return view;
        }
        
        private BasePanelView LoadPanelPrefab(PanelType panelType)
        {
            var view = _factoryPanels.Create(panelType);
            view.gameObject.transform.SetParent(_panelPull.transform,false);
            return view;
        }
    }
}