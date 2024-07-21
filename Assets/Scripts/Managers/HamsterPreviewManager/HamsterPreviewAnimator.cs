using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Managers.HamsterPreviewManager
{
    public class HamsterPreviewAnimator : MonoBehaviour
    {
        public event Action OnIdleStateEntered;

        private int IDLETYPE_HASH = Animator.StringToHash("idleType");
        private int IDLE_HASH = Animator.StringToHash("Idle");
        private int LOVE_HASH = Animator.StringToHash("Love");
        private int DANCE_HASH = Animator.StringToHash("Dance");
        private int SAD_HASH = Animator.StringToHash("Sad");
        private int FEED_HASH = Animator.StringToHash("Feed");
        private int BRUSH_HASH = Animator.StringToHash("Brush");
        private int FIRST_ENTER_HASH = Animator.StringToHash("FirstEnter");

        [SerializeField] private Animator _animator;

        private void OnEnable()
        {
            if (_animator.GetBehaviour<HamsterPreviewIdleState>() != null)
                _animator.GetBehaviour<HamsterPreviewIdleState>().OnStateEntered += IdleStateEntered;
        }

        private void OnDisable()
        {
            if(_animator.GetBehaviour<HamsterPreviewIdleState>() != null)
                _animator.GetBehaviour<HamsterPreviewIdleState>().OnStateEntered -= IdleStateEntered;
        }

        private void IdleStateEntered() => OnIdleStateEntered?.Invoke();

        internal void PlayDanceAnimation()
        {
            _animator.SetTrigger(DANCE_HASH);
        }

        internal void PlaySadAnimation()
        {
            _animator.SetTrigger(SAD_HASH);
        }

        internal void PlayLoveAnimation()
        {
            _animator.SetTrigger(LOVE_HASH);
        }

        internal void PlayHappyAnimation()
        {
            _animator.SetTrigger(BRUSH_HASH);
        }

        internal void PlayFeedAnimation()
        {
            _animator.SetTrigger(FEED_HASH);
        }

        internal void PlayFirstEnterAnimation()
        {
            _animator.SetTrigger(FIRST_ENTER_HASH);
        }

        internal void PlayIdleAnimation(float idleIndex = 0)
        {
            _animator.SetFloat(IDLETYPE_HASH, idleIndex);
            _animator.SetTrigger(IDLE_HASH);
        }
    }
}
