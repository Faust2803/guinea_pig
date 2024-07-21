using UnityEngine;

namespace Game.Jumper
{
    public class CameraFollowInPlane : MonoBehaviour
    {
        public Transform Target;
        public bool ClampByY;
        public Vector2 MinAndMaxYPos;
        public Vector2 ClampYInnerOffset = new Vector2(0.5f, 1f);

        [SerializeField] float minYLine = 2;
        public Vector3 offset = new Vector3(0, 0, -5f);
        [SerializeField] float smoothMove = .5f;
        [SerializeField] private float _sceneWidth = 8f;
        [SerializeField] private Camera _camera;

        private void Start()
        {
            _camera = GetComponent<Camera>();

            UpdateCameraFOV();
        }

        private void Update()
        {
            if (Target != null)
            {
                AlignToTarget();
            }
        }

        private float alignHeight;

        private float targetPosY =>
            Target.position.y;

        private float localPosY =>
            transform.position.y;

        private void AlignToTarget()
        {
            var velocity = 0f;
            alignHeight = Mathf.SmoothDamp(alignHeight,
                Mathf.Clamp(targetPosY, minYLine, float.MaxValue),
                ref velocity,
                smoothMove);

            var yRaw = offset.y + alignHeight;
            var y = ClampByY ? Mathf.Clamp(yRaw, MinAndMaxYPos.x + ClampYInnerOffset.x, MinAndMaxYPos.y + ClampYInnerOffset.y) : yRaw;
            var currentPos = new Vector3(offset.x, y, offset.z);
            transform.position = currentPos;
        }

        private void UpdateCameraFOV()
        {
            var startPosBottom = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 5f));
            
            float unitsPerPixel = _sceneWidth / Screen.width;
            
            //minYLine += minYLine * unitsPerPixel;
            float desiredHalfHeight = 0.5f * unitsPerPixel * Screen.height;

            _camera.orthographicSize = desiredHalfHeight;

            var endPosBottom = _camera.ViewportToWorldPoint(new Vector3(0.5f, 0f, 5f));
           
            offset += Vector3.up * (startPosBottom.y - endPosBottom.y);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(-5, minYLine), new Vector3(5, minYLine));
        }
    }
}