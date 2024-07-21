using DG.Tweening;
using UnityEngine;

namespace Managers.SoundManager
{
    public class SoundSmoothEffect
    {
        public bool IsValid => _isValid;
        
        private float _duration;
        private Ease _ease;
        private bool _isValid;

        public SoundSmoothEffect(float duration, Ease ease = Ease.Linear)
        {
            _duration = Mathf.Max(duration, 0f);
            _ease = ease;
            _isValid = !Mathf.Approximately(_duration, 0f);
        }
        
        public Sequence ApplyEffect(Sequence sequence, AudioSource source, float endValue)
        {
            var deltaVolume = Mathf.Abs(source.volume - endValue);
            var duration = _duration * deltaVolume;
            
            return sequence.Append(source.DOFade(endValue, duration).SetEase(_ease));
        }

        public Sequence ApplyEffect(Sequence sequence, AudioSource source, float startValue, float endValue)
        {
            var deltaVolume = Mathf.Clamp01(Mathf.Abs(startValue - endValue));
            var duration = _duration * deltaVolume;
            
            return sequence.Append(source.DOFade(endValue, duration).SetEase(_ease));
        }
    }
}