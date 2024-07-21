using UnityEngine;

namespace Common.HamsterPyramid.PlacedObjects
{
    public class PlacedObjectByVector : PlacedObjectBase
    {
        [SerializeField] private Vector3 _size = Vector3.one;
        
        public override Bounds CreateBounds()
        {
            return new Bounds(_centerPoint.position + _centerOffset, _size);
        }
    }
}