using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataModels.CollectionsData;
using DataModels.MoonBank;
using Managers;
using UnityEngine;
using UnityEngine.SceneManagement;
using Scene = UnityEngine.SceneManagement.Scene;

namespace Common.HamsterPyramid
{
    public interface IHamsterPyramidDataProvider
    {
        event Action<IHamsterVisualData> OnTargetChanged;
        event Action<IHamsterVisualData> OnAddHamster;
        
        bool TryGetData(out IReadOnlyCollection<IHamsterVisualData> pyramidData);
        bool TryGetMoonData(out MoonInfoData moonInfoData);
    }
    
    public class HamsterPyramidDataProvider : IHamsterPyramidDataProvider, IDisposable
    {
        public event Action<IHamsterVisualData> OnTargetChanged;
        public event Action<IHamsterVisualData> OnAddHamster;
        
        private INetworkManager _networkManager;
        private CollectionManager _collectionManager;
        private PlayerManager _playerManager;

        private PyramidCollectionData _pyramidData;
        private MoonInfoData _moonData;
        private bool _isInitialized;
        private List<IHamsterVisualData> _pyramidCollection = new List<IHamsterVisualData>();
        private IHamsterVisualData _activeHamster;

        // [Inject]
        public HamsterPyramidDataProvider(INetworkManager networkManager, CollectionManager collectionManager, PlayerManager playerManager)
        {
            _networkManager = networkManager;
            _collectionManager = collectionManager;
            _playerManager = playerManager;
            Initialize();
        }

        private async UniTaskVoid Initialize()
        {
            await UniTask.WaitUntil(() => _networkManager.IsAuthorized);
            
            _pyramidData = await _networkManager.Piramid();
            
            if (_pyramidData == null || _pyramidData.PyramidCollectionItems == null || _pyramidData.PyramidCollectionItems.Length == 0)
            {
                Debug.LogError($"[{nameof(HamsterPyramidController)}] Pyramid collection is empty.");
                return;
            }
            
            _collectionManager.SetupCollection(_pyramidData.PyramidCollectionItems);

            foreach (var itemData in _pyramidData.PyramidCollectionItems)
                _pyramidCollection.Add(itemData);
            
            _moonData = await _networkManager.GetMoonInfo();
            _activeHamster = _pyramidCollection[_pyramidCollection.Count - 1];
            _isInitialized = true;
            
            _playerManager.OnBuyCollectionItem += AddHamster;
            _playerManager.OnEquipedCollectionItem += SetTargetHamster;
            
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private async UniTaskVoid UpdateMoonData()
        {
            _moonData = await _networkManager.GetMoonInfo();
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            if(scene.name != Managers.Scene.Lobby.ToString()) return;

            UpdateMoonData();
        }

        public bool TryGetData(out IReadOnlyCollection<IHamsterVisualData> pyramidData)
        {
            pyramidData = _pyramidCollection;
            return _isInitialized;
        }

        public bool TryGetMoonData(out MoonInfoData moonInfoData)
        {
            moonInfoData = _moonData;
            return _isInitialized;
        }

        private void AddHamster(IHamsterVisualData hamsterViewData)
        {
            foreach (var pyramidItem in _pyramidCollection)
            {
                if (pyramidItem.Id == hamsterViewData.Id)
                {
                    Debug.LogError($"[{nameof(HamsterPyramidDataProvider)}] New collection item with id: {hamsterViewData.Id} is already exist in the map.");
                    return;
                }
            }
            
            _collectionManager.SetupCollectionItem(hamsterViewData);

            _pyramidCollection[_pyramidCollection.Count - 1] = hamsterViewData;
            _pyramidCollection.Add(_activeHamster);
            OnAddHamster?.Invoke(hamsterViewData);
        }

        private void SetTargetHamster(IHamsterVisualData hamsterViewData)
        {
            var i = 0;
            for (i = 0; i < _pyramidCollection.Count; i++)
            {
                if (_pyramidCollection[i].Id == hamsterViewData.Id)
                    break;
            }
            if (i == _pyramidCollection.Count - 1) return;
            
            var target = _pyramidCollection[i];
            _pyramidCollection[i] = _activeHamster;
            _activeHamster = target;
            _pyramidCollection[_pyramidCollection.Count - 1] = target;
            
            OnTargetChanged?.Invoke(_activeHamster);
        }

        public void Dispose()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;

            if(_playerManager == null) return;
            
            _playerManager.OnBuyCollectionItem -= AddHamster;
            _playerManager.OnEquipedCollectionItem -= SetTargetHamster;
        }
    }
}