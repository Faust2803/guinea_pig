using System;
using System.Collections;
using System.Runtime.CompilerServices;
using Cinemachine.Utility;
using DG.Tweening;
using UnityEngine;

namespace Common.HamsterPyramid.PyramidCamera
{
    public interface ICameraControllerInfo
    {
        public Vector3 CurrentPosition { get; }
        public CameraBounds CameraBounds { get; }
        public Camera Camera { get; }
    }
    
    public class CameraController : MonoBehaviour, ICameraControllerInfo
    {
        public Vector3 CurrentPosition => _transform.position;
        public CameraBounds CameraBounds => _cameraBounds;
        public Camera Camera => _mainCamera;
        
        [Header("References")]
        [SerializeField] private InputManager.InputManager _inputManager;
        [SerializeField] private Camera _mainCamera = null;
        [SerializeField] private CameraBounds _cameraBounds;
        [SerializeField] private Transform _transform;

        [Header("Movement settings")]
        [SerializeField] private float _moveSpeed = 4f;
        [SerializeField] private float _movementOmegaCoef = 1000f;
        [SerializeField] private float _movementSmoothness = 500f;
        [SerializeField, Range(0f, 1f)] private float _velocityMultiplier;
        
        [Header("Zoom settings")]
        // [SerializeField] private float _zoomSpeed = 4f;
        [SerializeField] private float _zoomOmegaCoef = 1000f;
        [SerializeField] private float _zoomSmoothness = 500f;
        [SerializeField, Range(0f, 1f)] private float _velocityZoomMultiplier;
        
        [Header("Constrain settings")]
        [SerializeField] private bool _constrainX = true;
        [SerializeField] private bool _constrainY = false;
        [SerializeField] private bool _constrainZ = true;
        [Space]
        [SerializeField] private bool _constrainBound = true;
        
        [Header("Snapping settings")]
        [SerializeField] private float _followSpeed = 4f;
        [SerializeField] private Ease _followEase = Ease.OutSine;
        [SerializeField] private float _zoomingSpeed = 15f;
        [SerializeField] private Ease _zoomEase = Ease.OutSine;
        
        [Header("Debug")]
        [SerializeField] private bool _isDraw;
        
        private float _deltaTimeBuffer;
        private Vector3 _movementConstrain = Vector3.up;

        private CameraState _cameraState = CameraState.None;
        private Vector3 _dragStartPosition;
        private Vector3 _cameraStartPosition;

        // smooth movement
        private Vector3 _movementVelocity = Vector3.zero;
        private Vector3 _targetPosition;

        private float _zoomVelocity = 0f;
        private float _targetZoom;

        // temp flag
        private SingleTouchPhase _touchPhase;

        private bool _isInitialized;
        private bool _isCanBreakFocusing = true;
        private bool _canHandleInput => _isInitialized && _isCanBreakFocusing;
        
        private Tweener _tweener;

        private void Awake()
        {
            SubscribeToInput();
            _targetPosition = _transform.position;
            _movementConstrain = new Vector3(_constrainX ? 0f : 1f, 
                                             _constrainY ? 0f : 1f, 
                                             _constrainZ ? 0f : 1f);

            _targetZoom = _mainCamera.orthographic ?  _mainCamera.orthographicSize : _mainCamera.fieldOfView;
        }

        public void Initialize()
        {
            _cameraBounds.Initialize();
            var pos = _cameraBounds.ClosestPoint(_transform.position);
            Initialize(pos);
        }

        public void Initialize(Bounds bounds)
        {
            var constrainBound = new Bounds(bounds.center, _constrainBound ? bounds.size.Multiply(_movementConstrain) : bounds.size);
            _cameraBounds.Initialize(constrainBound);
            var pos = _cameraBounds.Bounds.center;
            Initialize(pos);
        }
        
        public void Initialize(Bounds bounds, Vector3 position)
        {
            var constrainBound = new Bounds(bounds.center, _constrainBound ? bounds.size.Multiply(_movementConstrain) : bounds.size);
            _cameraBounds.Initialize(constrainBound);
            var pos = _cameraBounds.ClosestPoint(position);
            Initialize(pos);
        }

        private void Initialize(Vector3 position)
        {
            _transform.position = position;
            _targetPosition = position;
            _isInitialized = true;
        }

        public void UpdateBounds(Bounds bounds)
        {
            if(!_isInitialized) return;
            
            var constrainBound = new Bounds(bounds.center, _constrainBound ? bounds.size.Multiply(_movementConstrain) : bounds.size);
            _cameraBounds.Initialize(constrainBound);
            var pos = _cameraBounds.ClosestPoint(_transform.position);
            _targetPosition = pos;
            _transform.position = pos;
        }

        private Sequence _sequence;
        public float FocusingAt(Vector3 position, float targetZoom, bool immediate = false, Action callBack = null, bool isCanCanceled = false)
        {
            if (_cameraState == CameraState.Focusing)
            {
                _sequence?.Kill();
                SetEndValues();
            }
            else
            {
                _movementVelocity = Vector3.zero;
                _targetPosition = _transform.position;
                _touchPhase = SingleTouchPhase.Null;
                _targetZoom = _mainCamera.orthographic ?  _mainCamera.orthographicSize : _mainCamera.fieldOfView;
                _zoomVelocity = 0f;
            }
            
            var currentPos = _transform.position;
            var currentZoom = _mainCamera.orthographic ?  _mainCamera.orthographicSize : _mainCamera.fieldOfView;

            var posDif = _cameraBounds.ClosestPoint(position) - currentPos;
            var targetPos = currentPos + posDif.Multiply(_movementConstrain);

            if (immediate)
            {
                _transform.position = targetPos;
                if (_mainCamera.orthographic)
                    _mainCamera.orthographicSize = targetZoom;
                else
                    _mainCamera.fieldOfView = targetZoom;
                
                SetEndValues();
                return 0f;
            }

            _sequence = DOTween.Sequence();
            _cameraState = CameraState.Focusing;
            _isCanBreakFocusing = isCanCanceled;
            
            var distanceMove = Vector3.Distance(targetPos, currentPos);

            var durationMove = 0f;
            if (distanceMove > 0.01f)
            {
                durationMove = distanceMove / _followSpeed;
                _sequence.Join(_transform.DOMove(targetPos, durationMove).SetEase(_followEase));
            }
            
            var distanceZoom = Mathf.Abs(currentZoom - targetZoom);
            var durationZoom = 0f;
            
            if (distanceZoom > 0.01f)
            {
                durationZoom = distanceZoom / _zoomingSpeed;
            
                _sequence.Join(DOVirtual.Float(currentZoom, targetZoom, durationZoom, x =>
                {
                    if (_mainCamera.orthographic)
                        _mainCamera.orthographicSize = x;
                    else
                        _mainCamera.fieldOfView = x;
                }).SetEase(_zoomEase));
            }

            _sequence.AppendCallback(() =>
            {
                callBack?.Invoke();
                SetEndValues();
            });

            return Mathf.Max(durationMove, durationZoom);
        }

        public float FocusingAt(float targetZoom, bool immediate = false, Action callBack = null)
        {
            return FocusingAt(_transform.position, targetZoom, immediate, callBack, false);
        }
        
        public float FocusingAt(Vector3 position, bool immediate = false, Action callBack = null, bool isCanCanceled = false)
        {
            var currentZoom = _mainCamera.orthographic ?  _mainCamera.orthographicSize : _mainCamera.fieldOfView;
            return FocusingAt(position, currentZoom, immediate, callBack, isCanCanceled);
            // if (_cameraState == CameraState.Focusing)
            // {
            //     _tweener?.Kill();
            //     SetEndValues();
            // }
            // else
            // {
            //     _movementVelocity = Vector3.zero;
            //     _targetPosition = _transform.position;
            //     _touchPhase = SingleTouchPhase.Null;
            // }
            //
            // var currentPos = _transform.position;
            //
            // var posDif = _cameraBounds.ClosestPoint(position) - currentPos;
            // var targetPos = currentPos + posDif.Multiply(_movementConstrain);
            // var distance = Vector3.Distance(targetPos, currentPos);
            //
            // if(distance <= 0.01f) return 0f;
            //
            // if (immediate)
            // {
            //     _transform.position = targetPos;
            //     SetEndValues();
            //     return 0f;
            // }
            //
            // var duration = distance / _followSpeed;
            // _cameraState = CameraState.Focusing;
            // _isCanBreakFocusing = isCanCanceled;
            //
            // _tweener = _transform.DOMove(targetPos, duration).SetEase(_followEase).OnComplete(SetEndValues);
            //
            // return duration;
        }

        public void ZoomAt(float value, bool immediate = false, Action zoomCallback = null)
        {
            if(_zoomCoroutine != null)
                StopCoroutine(_zoomCoroutine);
            
            if (immediate)
            {
                if (_mainCamera.orthographic)
                    _mainCamera.orthographicSize = value;
                else
                    _mainCamera.fieldOfView = value;
                
                _targetZoom = value;
                zoomCallback?.Invoke();
                return;
            }
            
            _targetZoom = value;
            _zoomCoroutine = StartCoroutine(WaitZoom(zoomCallback));
        }

        private Coroutine _zoomCoroutine;
        private IEnumerator WaitZoom(Action zoomCallback)
        {
            yield return new WaitUntil(IsCameraZoomNotReached);
            yield return null;
            zoomCallback?.Invoke();
            _zoomCoroutine = null;
        }
        
        private void SetEndValues()
        {
            _targetPosition = _transform.position;
            _touchPhase = SingleTouchPhase.Null;
            _isCanBreakFocusing = true;
            _cameraState = CameraState.None;
            _targetZoom = _mainCamera.orthographic ?  _mainCamera.orthographicSize : _mainCamera.fieldOfView;
        }

        private void SubscribeToInput()
        {
            _inputManager.OnSingleTouchBegin += BeginSingleTouch;
            _inputManager.OnSingleTouchHold  += HoldSingleTouch;
            _inputManager.OnSingleTouchEnded += EndSingleTouch;
        }

        private void UnsubscribeFromInput()
        {
            _inputManager.OnSingleTouchBegin -= BeginSingleTouch;
            _inputManager.OnSingleTouchHold  -= HoldSingleTouch;
            _inputManager.OnSingleTouchEnded -= EndSingleTouch;
        }
        
        private void BeginSingleTouch(Vector3 touchPosition)
        {
            if (_canHandleInput == false || _inputManager.IsPointerOverUiObject())
                return;

            if (_cameraState == CameraState.Focusing && _isCanBreakFocusing)
            {
                _tweener?.Kill();
                SetEndValues();
            }

            _touchPhase = SingleTouchPhase.Begin;
            var ray = _mainCamera.ScreenPointToRay(touchPosition);
            _dragStartPosition = _cameraBounds.GroundRayCast(ray).Multiply(_movementConstrain);
            // _movementVelocity = Vector3.zero;
        }
        
        private void HoldSingleTouch(Vector3 touchPosition)
        {
            if (_canHandleInput == false)
                return;

            if (_touchPhase != SingleTouchPhase.Begin)
                return;

            var ray = _mainCamera.ScreenPointToRay(touchPosition);
            var currentDragPosition = _cameraBounds.GroundRayCast(ray).Multiply(_movementConstrain);

            var direction = _dragStartPosition - currentDragPosition;
            if (direction.magnitude < float.Epsilon) // no movement needed
            {
                return;
            }

            var currentPosition = _transform.position;
            _targetPosition = currentPosition + direction * _moveSpeed;
            // _targetPosition = _cameraBounds.ClosestPoint(_targetPosition);
        }

        private void EndSingleTouch(Vector3 touchPosition)
        {
            _targetPosition = _cameraBounds.ClosestPoint(_targetPosition);
            _touchPhase = SingleTouchPhase.Ended;
            _dragStartPosition = Vector3.zero;
        }
        
        private void LateUpdate()
        {
            var dt = Time.deltaTime;
            if (_cameraState != CameraState.Focusing && Time.timeScale != 0)
            {
                _deltaTimeBuffer = (_deltaTimeBuffer + dt) * 0.5f;
                var currentPosition = _transform.position;
                var movementOmega = _movementSmoothness / (_deltaTimeBuffer * _movementOmegaCoef);

                _movementVelocity *= _velocityMultiplier;
                var springPosition = Step(currentPosition, _targetPosition, ref _movementVelocity, movementOmega, dt);

                if (springPosition.IsNaN())
                    _movementVelocity = Vector3.zero;
                else
                    _transform.position = _cameraBounds.ClosestPoint(springPosition);
                
                // if(!IsCameraZoomNotReached()) return;
                
                var currentZoom = _mainCamera.orthographic
                    ? _mainCamera.orthographicSize
                    : _mainCamera.fieldOfView;
                
                _zoomVelocity *= _velocityZoomMultiplier;
                var zoomOmega = _zoomSmoothness / (_deltaTimeBuffer * _zoomOmegaCoef);
                var springZoom = Step(currentZoom, _targetZoom, ref _zoomVelocity, zoomOmega);

                if (_mainCamera.orthographic)
                    _mainCamera.orthographicSize = springZoom;
                else
                    _mainCamera.fieldOfView = springZoom;
            }
        }
        
        private bool IsCameraZoomNotReached()
        {
            if (_mainCamera.orthographic)
            {
                return Mathf.Abs(_mainCamera.orthographicSize - _targetZoom) > 0.001f;
            }
            
            return Mathf.Abs(_mainCamera.fieldOfView - _targetZoom) > 0.001f;

        }

         private void OnDestroy()
         {
             UnsubscribeFromInput();
         }

         private void OnDrawGizmos()
         {
             if(!_isDraw) return;
            
             Gizmos.color = Color.red;
             Gizmos.DrawWireSphere(transform.position, 0.5f);
         }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static float Step(float current, float target, ref float velocity, float omega)
        {
            var dt = Time.deltaTime;
            var n1 = velocity - (current - target) * (omega * omega * dt);
            var n2 = 1 + omega * dt;
            velocity = n1 / (n2 * n2);
            return current + velocity * dt;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Vector3 Step(Vector3 current, Vector3 target, ref Vector3 velocity, float omega, float dt)
        {
            var n1 = velocity - (current - target) * (omega * omega * dt);
            var n2 = 1 + omega * dt;
            velocity = n1 / (n2 * n2);
            return current + velocity * dt;
        }
    }
}