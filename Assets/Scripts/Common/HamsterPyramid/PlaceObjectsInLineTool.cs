using System.Collections.Generic;
using Common.HamsterPyramid.PlacedObjects;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.HamsterPyramid
{
    public class PlaceObjectsInLineTool : MonoBehaviour
    {
        public float Spacing => _spacing;
        
        [SerializeField] private Transform _rootPoint;
        [SerializeField] private PlacedDirection _placedDirection;
        [SerializeField] private PlacedAlignment _placedAlignment;
        [SerializeField] private float _spacing = 0f;
        [SerializeField] private Vector3 _offsetStartPoint = Vector3.zero;
        private List<IPlacedObject> _objectsToPlace = new List<IPlacedObject>();
        private Bounds _bounds = new Bounds();
        private IPlacedOffset _placedOffset = new PlacedOffsetEmpty();

        public void PlaceObjects(ICollection<IPlacedObject> objectsToPlace)
        {
            _placedOffset = new PlacedOffsetEmpty();
            Place(objectsToPlace);
        }
        
        public void PlaceObjects(ICollection<IPlacedObject> objectsToPlace, IPlacedOffset offset)
        {
            _placedOffset = offset;
            Place(objectsToPlace);
        }

        private void Place(ICollection<IPlacedObject> objectsToPlace)
        {
            _objectsToPlace.Clear();
            if(objectsToPlace.Count == 0) return;

            var offsets = new Vector3[objectsToPlace.Count];
            
            var fullSize = Vector3.zero;
            var index = 0;
            
            foreach (var objToPlace in objectsToPlace)
            {
                fullSize += objToPlace.Size;
                _objectsToPlace.Add(objToPlace);
                var additionalOffset = _placedOffset.GetPositionOffset(objToPlace, index + 1, objectsToPlace.Count);
                offsets[index++] = additionalOffset;
                fullSize += additionalOffset;
            }

            var spacing = Vector3.one * _spacing * (objectsToPlace.Count - 1);
            fullSize += spacing;

            var direction = GetDirection(_placedDirection);
            var alignment = GetAlignment(_placedAlignment);
            var position = _rootPoint.position + _offsetStartPoint;

            // direction *= order;
            //TODO: Use order to change direction.
            var sizeWithDirection = fullSize.Multiply(direction);
            // var halfSizeWithDirection = sizeWithDirection * 0.5f;
            // var center = position + (alignment * halfSizeWithDirection);
            // var startPoint = center + (direction * (0.5f * alignment));
            // var alignmentRemap = (alignment + 1f) * 0.5f;

            // var koef = Mathf.Approximately(alignment, 0f) ? 0.5f * order : order;
            // var koef = Mathf.Approximately(alignment, 0f) ? -0.5f : alignment;
            
            // var startPoint = position + (sizeWithDirection * (-1 * alignmentRemap));
            var startPoint = position - (sizeWithDirection * alignment);
            //TODO: Placed objects in line.

            var obj = _objectsToPlace[0];
            var size = direction.Multiply(obj.Size);
            var halfSize = size * 0.5f;
            var nextPosition = startPoint + halfSize + offsets[0];
            spacing = direction * _spacing;
            var prevOffset = halfSize + spacing;
            obj.SetPosition(nextPosition);

            for (int i = 1; i < _objectsToPlace.Count; i++)
            {
                obj = _objectsToPlace[i];
                size = direction.Multiply(obj.Size);
                halfSize = size * 0.5f;
                nextPosition = nextPosition + halfSize + prevOffset + offsets[i];;
                prevOffset = halfSize + spacing;
                obj.SetPosition(nextPosition);
            }

            var firstObjBounds = _objectsToPlace[0].GetBound();
            _bounds = new Bounds(firstObjBounds.center, firstObjBounds.size);

            for (int i = 1; i < _objectsToPlace.Count; i++)
                _bounds.Encapsulate(_objectsToPlace[i].GetBound()); 
        }

        public Bounds GetBounds()
        {
            var bounds = new Bounds(_bounds.center, _bounds.size);
            return bounds;
        }

        private Vector3 GetDirection(PlacedDirection direction)
        {
            return direction switch
            {
                PlacedDirection.Up => Vector3.up,
                PlacedDirection.Down => Vector3.down,
                PlacedDirection.Right => Vector3.right,
                PlacedDirection.Left => Vector3.left,
                PlacedDirection.Forward => Vector3.forward,
                PlacedDirection.Back => Vector3.back,
                _ => Vector3.up
            };
        }

        private float GetAlignment(PlacedAlignment alignment)
        {
            return alignment switch
            {
                PlacedAlignment.PointStart => 0f,
                PlacedAlignment.PointCenter => 0.5f,
                PlacedAlignment.PointEnd => 1f,
                _ => 0.5f
            };
        }

        private enum PlacedDirection
        {
            Up,
            Down,
            Right,
            Left,
            Forward,
            Back
        }

        private enum PlacedAlignment
        {
            PointStart,
            PointCenter,
            PointEnd
        }
    }

    public interface IPlacedOffset
    {
        Vector3 GetPositionOffset(IPlacedObject placedObject, int number = 1, int amount = 1);
    }

    public class PlacedOffsetEmpty : IPlacedOffset
    {
        public Vector3 GetPositionOffset(IPlacedObject placedObject, int number = 1, int amount = 1)
        {
            return Vector3.zero;
        }
    }

    public class PlacedRandomOffset : IPlacedOffset
    {
        private Vector3 _min;
        private Vector3 _max;
        
        public PlacedRandomOffset(Vector3 min, Vector3 max)
        {
            _min = min;
            _max = max;
        }
        public Vector3 GetPositionOffset(IPlacedObject placedObject, int number = 1, int amount = 1)
        {
            return new Vector3(Random.Range(_min.x, _max.x), Random.Range(_min.y, _max.y), Random.Range(_min.z, _max.z));
        }
    }
}