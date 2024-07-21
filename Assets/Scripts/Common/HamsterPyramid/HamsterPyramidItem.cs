using System;
using Common.HamsterPyramid.PlacedObjects;
using DataModels.CollectionsData;
using Game.Jumper;
using UnityEngine;

namespace Common.HamsterPyramid
{
    public class HamsterPyramidItem : MonoBehaviour
    {
        public PlacedObjectBase HamsterPlacedObject => _placedObject;
        public int Id => _viewData.Id;
        public HamsterPyramidState CurrentState => _currentState;
        public IHamsterVisualData VisualData => _viewData;

        private HamsterPyramidState _currentState = HamsterPyramidState.None;
        private IHamsterVisualData _viewData;
        
        [SerializeField] private PlacedObjectBase _placedObject;
        [SerializeField] private PlayerVisualView _playerVisualView;
        [SerializeField] private AnimatorStateInfo[] _animatorInfos = Array.Empty<AnimatorStateInfo>();
        [SerializeField] private GameObject _mop;

        public void Setup(IHamsterVisualData data)
        {
            _playerVisualView.SetupCollection(data);
            _viewData = data;
        }

        public void SetState(HamsterPyramidState state)
        {
            _currentState = state;
            _mop.SetActive(state == HamsterPyramidState.Center || 
                           state == HamsterPyramidState.Left || 
                           state == HamsterPyramidState.Right);

            foreach (var animatorInfo in _animatorInfos)
            {
                if (!animatorInfo.Animator.isActiveAndEnabled) continue;

                var animatorStateName = animatorInfo.DefaultStateName;

                foreach (var pair in animatorInfo.StatePairName)
                {
                    if (pair.HamsterState != state) continue;

                    animatorStateName = pair.StateName;

                    break;
                }

                animatorInfo.Animator.Play(animatorStateName, 0, 0f);
            }
        }

        [Serializable]
        private class AnimatorStateInfo
        {
            public Animator Animator;
            public string DefaultStateName = "Empty";
            public StatePairAnimatorStateName[] StatePairName;
        }
        
        [Serializable]
        private class StatePairAnimatorStateName
        {
            public HamsterPyramidState HamsterState;
            public string StateName;
        }
    }

    public enum HamsterPyramidState
    {
        None = 0,
        
        Center = 10,
        Left,
        Right,
        Left_finish,
        Right_finish
    }
}