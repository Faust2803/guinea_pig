using Common.HamsterPyramid.PlacedObjects;
using Common.HamsterPyramid.PyramidCamera;
using UnityEngine;

namespace Common.HamsterPyramid.Test
{
    public class TestPlacedObjTool : MonoBehaviour
    {
        [SerializeField] private PlacedObjectBase _prefab;
        [SerializeField] private int _count = 10;
        [SerializeField] private PlaceObjectsInLineTool _tool;
        [SerializeField] private CameraController _cameraController;
        
        private IPlacedObject[] _placedObjects;

        private void Start()
        {
            _placedObjects = new IPlacedObject[_count];
            
            for (int i = 0; i < _placedObjects.Length; i++)
                _placedObjects[i] = Instantiate(_prefab, transform);

        }

        [ContextMenu(nameof(PlaceObjects))]
        private void PlaceObjects()
        {
            _tool.PlaceObjects(_placedObjects);
            var baunce = _tool.GetBounds();
            _cameraController.Initialize(baunce);
            _cameraController.FocusingAt(_placedObjects[_placedObjects.Length - 1].GetBound().center, true);
        }

        [ContextMenu(nameof(FocusAtLast))]
        private void FocusAtLast()
        {
            _cameraController.FocusingAt(_placedObjects[_placedObjects.Length - 1].GetBound().center);
        }
    }
}