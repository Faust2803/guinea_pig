using UnityEngine;

namespace Game.Visual
{
    public class Prodecular2DPlaneAnimation : MonoBehaviour
    {
        public bool IsPlaying = true;
        public bool RandomOffsetOnStart = true;
        public float SpeedPlay = 2f;
        public float ScaleValue = 1f;

        [SerializeField] AnimationCurve rotate;
        [SerializeField] AnimationCurve scale;

        private Vector3 _defaultLocalScale = Vector3.zero;

        private float time =>
            (Time.time + randomOffset) * SpeedPlay;

        private float randomOffset;

        private void Start()
        {
            _defaultLocalScale = transform.localScale;

            if (RandomOffsetOnStart)
                randomOffset = Random.value;
        }

        private void FixedUpdate()
        {
            transform.localScale = _defaultLocalScale + Vector3.one * ScaleValue * scale.Evaluate(time);
            transform.localRotation *= Quaternion.Euler(0, 0, rotate.Evaluate(time));
        }
    }
}