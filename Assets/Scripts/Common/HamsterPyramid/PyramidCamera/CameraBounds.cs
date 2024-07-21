using System;
using UnityEngine;

namespace Common.HamsterPyramid.PyramidCamera
{
    public class CameraBounds : MonoBehaviour
    {
        public Bounds Bounds => _bounds;
        
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _transform;
        [SerializeField] private Vector3 _boundsCenter = default;
        [SerializeField] private Vector3 _boundsSize = default;
        [SerializeField] private Vector3 _offsetSizeBound = Vector3.zero;
        [SerializeField] private bool _isDraw;
        
        private Plane _plane;
        private Bounds _bounds = new Bounds();
        private Bounds _boundOfCameraView;

        private void Start()
        {
            var centerPoint = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0f);
            _boundOfCameraView = new Bounds(centerPoint, new Vector3(Screen.width, Screen.height, 0f));
        }

        public void Initialize()
        {
            var boundsCenter = _boundsCenter + _transform.position;
            var boundsSize = _boundsSize.Multiply(_transform.localScale);
            // var bounds = new Bounds(boundsCenter, boundsSize);
            SetBound(boundsCenter, boundsSize);
        }

        public void Initialize(Bounds bounds)
        {
            // _plane = new Plane(-_mainCamera.gameObject.transform.forward, bounds.center);
            SetBound(bounds.center, bounds.size);
        }

        private void SetBound(Vector3 center, Vector3 size)
        {
            var sizeWithOffset = new Vector3(Mathf.Max(0, size.x + _offsetSizeBound.x),
                                             Mathf.Max(0, size.y + _offsetSizeBound.y),
                                             Mathf.Max(0, size.z + _offsetSizeBound.z));
            _bounds = new Bounds(center, sizeWithOffset);
            _plane = new Plane(-_mainCamera.gameObject.transform.forward, center);
        }
        
        public Vector3 ClosestPoint(Vector3 point) => _bounds.Contains(point) ? point : _bounds.ClosestPoint(point);
        
        public Vector3 GroundRayCast(Ray ray)
        {
            _plane.Raycast(ray, out var distance);
            return ray.GetPoint(distance);
        }

        public bool InSight(Vector3 position)
        {
            var posInCameraSpace = _mainCamera.WorldToScreenPoint(position);
            posInCameraSpace.z = 0f;

            return _boundOfCameraView.Contains(posInCameraSpace);
        }

        private void OnDrawGizmos()
        {
            if(!_isDraw) return;
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size);
        }
    }
}