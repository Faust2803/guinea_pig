using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.HamsterPyramid.InputManager
{
    public class InputManager : MonoBehaviour, IInputManager
    {
        public bool IgnoreInput { get; set; } = false;

        public event Action<Vector3> OnSingleTouchBegin;
        public event Action<Vector3> OnSingleTouchHold;
        public event Action<Vector3> OnSingleTouchEnded;

        [SerializeField, Range(0.1f,  1.0f)] float touchEndDelay = 0.35f;
        [SerializeField] private LayerMask _layerMaskUI;
        private float _beginSingleTouchTime;
        private int _touchCount;
        private EventSystem _eventSystem = null;
        private CurrentInputSource _currentInputSource = CurrentInputSource.None;

        private void Awake()
        {
            _eventSystem = FindObjectOfType<EventSystem>();
            if (_eventSystem == null)
            {
                Debug.LogError("EventSystem not found on scene! Add it with Create/UI/Event System menu.");
                this.enabled = false;
            }
        }
        
        private void Update()
        {
            if (IgnoreInput) return;
            _touchCount = Input.touchCount;
            
            SingleTouchProcessing();
        }

        private void SingleTouchProcessing()
        {
            if (_currentInputSource == CurrentInputSource.PC || _currentInputSource == CurrentInputSource.None)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    _currentInputSource = CurrentInputSource.PC;
                    _beginSingleTouchTime = Time.time;
                    OnSingleTouchBegin?.Invoke(Input.mousePosition);
                }
                else if (Input.GetMouseButton(0) && _currentInputSource == CurrentInputSource.PC)
                {
                    OnSingleTouchHold?.Invoke(Input.mousePosition);
                }
                else if (Input.GetMouseButtonUp(0) && _currentInputSource == CurrentInputSource.PC)
                {
                    _beginSingleTouchTime = 0f;
                    _currentInputSource = CurrentInputSource.None;
                    OnSingleTouchEnded?.Invoke(Input.mousePosition);
                }
            }
            else if (_currentInputSource == CurrentInputSource.Mobile || _currentInputSource == CurrentInputSource.None)
            {
                var touch = new Touch
                {
                    phase = TouchPhase.Canceled
                };

                if (_touchCount == 1)
                    touch = Input.GetTouch(0);
                else if (_beginSingleTouchTime <= 0)
                    return;

                switch (touch.phase)
                {
                    // case TouchPhase.Began when _beginSingleTouchTime > float.Epsilon &&
                    //                            Time.time - _beginSingleTouchTime < touchEndDelay:
                    //     return;

                    case TouchPhase.Began:
                        _beginSingleTouchTime = Time.time;
                        _currentInputSource = CurrentInputSource.Mobile;
                        OnSingleTouchBegin?.Invoke(touch.position);

                        break;

                    case TouchPhase.Stationary:
                    case TouchPhase.Moved:
                        if (_currentInputSource == CurrentInputSource.Mobile)
                            OnSingleTouchHold?.Invoke(touch.position);

                        break;

                    case TouchPhase.Canceled:
                    case TouchPhase.Ended:
                        if (_currentInputSource != CurrentInputSource.Mobile)
                            break;

                        _beginSingleTouchTime = 0f;
                        _currentInputSource = CurrentInputSource.None;
                        OnSingleTouchEnded?.Invoke(touch.position);

                        break;

                    default: break;
                }
            }
        }

        public bool IsPointerOverUiObject() 
        {
            // var blockOnLayers = LayerMask.GetMask("UI");
            var blockOnLayers = _layerMaskUI;
            var eventDataCurrentPosition = new PointerEventData(EventSystem.current)
            {
                position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
            };
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current?.RaycastAll(eventDataCurrentPosition, results);
            return results.Any(i => blockOnLayers == (blockOnLayers | 1 << i.gameObject.layer));
            // return results.Count > 0;
        }

        private enum CurrentInputSource
        {
            None,
            Mobile,
            PC
        }

        

       
    }
}