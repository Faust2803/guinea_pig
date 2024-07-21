using Common.HamsterPyramid.PlacedObjects;
using TMPro;
using UnityEngine;

namespace Common.HamsterPyramid
{
    public class MoonPyramidItem : MonoBehaviour
    {
        public PlacedObjectBase PlacedObject => _placedObject;
        [SerializeField] private PlacedObjectBase _placedObject;
        [SerializeField] private TMP_Text _counterText;

        public void Init(string count)
        {
            _counterText.text = count;
        }
    }
}