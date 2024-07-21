using DataModels.CollectionsData;
using Game.Jumper;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace Managers.HamsterPreviewManager
{
    public class HamsterPreviewManager : MonoBehaviour
    {
        public event Action OnHamsterEnteredIdleState;

        [SerializeField] private GameObject _previewParent;
        [SerializeField] private PlayerVisualView _playerVisualView;
        [SerializeField] private HamsterPreviewAnimator _hamsterPreviewAnimator;

        [Inject] private PlayerManager _playerManager;

        private void OnEnable() => _hamsterPreviewAnimator.OnIdleStateEntered += OnIdleStateEntered;

        private void OnDisable() => _hamsterPreviewAnimator.OnIdleStateEntered -= OnIdleStateEntered;

        private void OnIdleStateEntered() => OnHamsterEnteredIdleState?.Invoke();

        private void Start() => ShowPreview(false);

        public void ShowPreview(bool shouldBeShown) => _previewParent.SetActive(shouldBeShown);

        public void UpdatedHamsterPreview() => _playerVisualView.SetupCollection(_playerManager.CurrentCollectionItem);
        public void UpdatedHamsterPreview(CollectionItemDataModel collectionItemDataModel) => _playerVisualView.SetupCollection(collectionItemDataModel);

        public void PlayPlayWithAnimation() => _hamsterPreviewAnimator.PlayLoveAnimation();
        public void PlayDiscoAnimation() => _hamsterPreviewAnimator.PlayDanceAnimation();
        public void PlayBrushAnimation() => _hamsterPreviewAnimator.PlayHappyAnimation();
        public void PlayFeedAnimation() => _hamsterPreviewAnimator.PlayFeedAnimation();
        public void PlaySadAnimation() => _hamsterPreviewAnimator.PlaySadAnimation();
        public void PlayFirstEnterAnimation() => _hamsterPreviewAnimator.PlayFirstEnterAnimation();
        public void PlayIdleAnimation() => _hamsterPreviewAnimator.PlayIdleAnimation();
    }
}
