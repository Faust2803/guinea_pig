using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Common.HamsterPyramid.PlacedObjects
{
    public class PlacedObjectByMeshSize : PlacedObjectBase
    {
        [SerializeField] private bool _drawWithoutOffset;
        [SerializeField] private Color _colorDrawWithoutOffset = Color.cyan;
        
        [Header("References")]
        [SerializeField] private List<MeshRenderer> _meshRendererList = new List<MeshRenderer>();
        [SerializeField] private List<SkinnedMeshRenderer> _skinnedMeshList = new List<SkinnedMeshRenderer>();
        
        [Header("Parameters")]
        [SerializeField] private Vector3 _offset = Vector3.zero;
        [SerializeField] private bool _isCenterInPoint;

        public override Bounds CreateBounds()
        {
            var center = transform.position;
            var size = Vector3.zero;
            
            if (_meshRendererList.Count > 0)
            {
                var meshBounds = _meshRendererList[0].bounds;
                center = meshBounds.center;
                size = meshBounds.size;//.Multiply(_skinnedMeshList[0].transform.lossyScale);
            } else if (_skinnedMeshList.Count > 0)
            {
                var skinBounds = _skinnedMeshList[0].bounds;
                center = skinBounds.center;
                size = skinBounds.size;//.Multiply(_skinnedMeshList[0].transform.lossyScale);
            }

            var bounds = new Bounds(center, size);
            
            foreach (var meshFilter in _meshRendererList)
            {
                if (meshFilter == null || !meshFilter.gameObject.activeSelf) continue;

                // var bound = new Bounds(meshFilter.bounds.center, meshFilter.bounds.size.Multiply(meshFilter.transform.lossyScale));
                bounds.Encapsulate(meshFilter.bounds);
            }

            foreach (var meshRenderer in _skinnedMeshList)
            {
                if(meshRenderer == null || !meshRenderer.gameObject.activeSelf) continue;
                
                // var bound = new Bounds(meshRenderer.bounds.center, meshRenderer.bounds.size.Multiply(meshRenderer.transform.lossyScale));

                bounds.Encapsulate(meshRenderer.bounds);
            }

            bounds.size += _offset;

            if (_isCenterInPoint)
                bounds.center = _centerPoint.position + _centerOffset;
            else 
                _centerOffset = _centerPoint.position - bounds.center;

            return bounds;
        }

#if UNITY_EDITOR
        [ContextMenu(nameof(GetAllMeshes))]
        private void GetAllMeshes()
        {
            if(_centerPoint == null) return;

            _meshRendererList.Clear();
            var meshFilters = _centerPoint.GetComponentsInChildren<MeshRenderer>(true);

            foreach (var meshFilter in meshFilters)
            {
                if(meshFilter != null)
                    _meshRendererList.Add(meshFilter);
            }
            
            _skinnedMeshList.Clear();
            var skinnedMeshes = _centerPoint.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            
            foreach (var skinnedMesh in skinnedMeshes)
            {
                if(skinnedMesh != null)
                    _skinnedMeshList.Add(skinnedMesh);
            }
            
            EditorUtility.SetDirty(gameObject);
        }
#endif
        
        protected override void DrawAdditional()
        {
            if(!_drawWithoutOffset) return;
            
            Gizmos.color = _colorDrawWithoutOffset;
            Gizmos.DrawWireCube(_bounds.center, _bounds.size - _offset);
        }
    }
}