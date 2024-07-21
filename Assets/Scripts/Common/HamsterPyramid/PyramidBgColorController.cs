using Common.HamsterPyramid.PyramidCamera;
using UnityEngine;

namespace Common.HamsterPyramid
{
    public class PyramidBgColorController : MonoBehaviour
    {
        [SerializeField] private PyramidHeightBgColorData _colorData;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private bool _useCameraBounds;

        private float _prevY = -1000f;
        private Color _currentColor;
        private float _minPosY = 0f;
        private float _maxPosY = 10f;

        public void SetRange(float minPosY, float maxPosY)
        {
            _minPosY = minPosY;
            _maxPosY = maxPosY;
        }
        
        private void Update()
        {
            if(Mathf.Approximately(_prevY, _cameraController.CurrentPosition.y)) return;

            if (_useCameraBounds)
                ChangeColorByBounds();
            else 
                ChangeColorByRange();
        }

        private void ChangeColorByBounds()
        {
            var bounds = _cameraController.CameraBounds.Bounds;
            var minValue = bounds.min.y;
            var maxValue = bounds.max.y;
            var posCameraY = Mathf.Clamp(_cameraController.CurrentPosition.y, minValue, maxValue);

            ChangeColor(posCameraY, minValue, maxValue);

        }

        private void ChangeColorByRange()
        {
            var bounds = _cameraController.CameraBounds.Bounds;
            var minValue = bounds.min.y;
            var maxValue = bounds.max.y;
            var posCameraY = Mathf.Clamp(_cameraController.CurrentPosition.y, minValue, maxValue);
            
            ChangeColor(posCameraY, _minPosY, _maxPosY);
        }

        private void ChangeColor(float posCameraY, float minValue, float maxValue)
        {
            var t = Extensions.InverseLerp(minValue, maxValue, posCameraY);
            var color = _colorData.GetColorLerp(t);
            _cameraController.Camera.backgroundColor = color;

            _prevY = posCameraY;
        }
    }
}