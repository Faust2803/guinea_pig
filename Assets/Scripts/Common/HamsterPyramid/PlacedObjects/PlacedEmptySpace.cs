using UnityEngine;

namespace Common.HamsterPyramid.PlacedObjects
{
    public class PlacedEmptySpace : IPlacedObject
    {
        public Vector3 Size => _bounds.size;

        private Bounds _bounds;
        public PlacedEmptySpace(Vector3 size)
        {
            _bounds = new Bounds();
            _bounds.size = size;
        }

        public Bounds GetBound()
        {
            return _bounds;
        }

        public void SetPosition(Vector3 position)
        {
            _bounds.center = position;
        }
    }
}