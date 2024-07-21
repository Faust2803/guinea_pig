using UnityEditor;
using UnityEngine;

namespace Common.HamsterPyramid.PlacedObjects
{
    public interface IPlacedObject
    {
        Vector3 Size { get; }
        Bounds GetBound();
        void SetPosition(Vector3 position);
    }
    
    public abstract class PlacedObjectBase : MonoBehaviour, IPlacedObject
    {
        public Vector3 Size => _bounds.size;
        
        [SerializeField] protected Transform _centerPoint;
        [SerializeField] protected Vector3 _centerOffset = Vector3.zero;
        [SerializeField] protected Bounds _bounds = new Bounds();

        public virtual Bounds GetBound()
        {
            return _bounds;
        }

        public virtual Bounds GetBounds(Vector3 position)
        {
            SetPosition(position);
            return GetBound();
        }

        public virtual void SetPosition(Vector3 position)
        {
            _centerPoint.position = position - _centerOffset;
            _bounds.center = position;
        }
        
        public abstract Bounds CreateBounds();

#if UNITY_EDITOR
        [ContextMenu(nameof(ConstructBounds))]
        private void ConstructBounds()
        {
            _bounds = CreateBounds();
            EditorUtility.SetDirty(gameObject);
        }
#endif

        [Header("Debug Drawing")]
        [SerializeField] protected bool _isDraw;
        [SerializeField] protected bool _drawOriginCenter;
        [SerializeField] protected bool _updateBounsValues;
        [SerializeField] private float _radiusSphere = 0.1f;
        
        [SerializeField] private Color _colorOriginBound = Color.yellow;
        [SerializeField] private Color _colorBound = Color.green;
        [SerializeField] private Color _colorCenter = Color.red;

        private void OnDrawGizmos()
        {
            if(!_isDraw) return;

            if (_centerPoint == null)
            {
                _isDraw = false;
                return;
            }

            var pos = _centerPoint.position;
            var bound = CreateBounds();
            
            if(_updateBounsValues) 
                _bounds = bound;

            Gizmos.color = _colorBound;
            Gizmos.DrawWireCube(bound.center, bound.size);

            Gizmos.color = _colorCenter;
            Gizmos.DrawSphere(bound.center, _radiusSphere);

            if (_drawOriginCenter && _centerOffset != Vector3.zero)
            {
                Gizmos.color = _colorOriginBound;
                Gizmos.DrawWireCube(pos, bound.size);
                Gizmos.DrawSphere(pos, _radiusSphere);
            }
            
            DrawAdditional();
        }

        protected virtual void DrawAdditional()
        {
            
        }
    }
}