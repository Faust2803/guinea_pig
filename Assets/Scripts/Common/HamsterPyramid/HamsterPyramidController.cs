using System;
using System.Collections.Generic;
using Common.HamsterPyramid.PlacedObjects;
using Common.HamsterPyramid.PyramidCamera;
using DataModels.CollectionsData;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

namespace Common.HamsterPyramid
{
    public class HamsterPyramidController : MonoBehaviour
    {
        public Color CurrentColorBg => _cameraController.Camera.backgroundColor;
        public bool HaveMoon => _withMoon;
        public Bounds MoonBounds => _withMoon ? _moonItem.PlacedObject.GetBound() : new Bounds();
        public Bounds TargetHamsterBound => _targetHamster.HamsterPlacedObject.GetBound();
        public ICameraControllerInfo CameraInfo => _cameraController;
        
        [Inject] private IHamsterPyramidDataProvider _dataProvider;
        
        // [Inject] private INetworkManager _networkManager;
        // [Inject] private CollectionManager _collectionManager;
        // [Inject] private PlayerManager _playerManager;

        [SerializeField] private PlaceObjectsInLineTool _placedTool;
        [SerializeField] private PyramidBgColorController _bgColorController;
        [SerializeField] private HamsterPyramidItem _hamsterPrefab;
        [SerializeField] private MoonPyramidItem _moonPyramidPrefab;
        [SerializeField] private CameraController _cameraController;
        [SerializeField] private Transform _parentForHamsterItem;
        [SerializeField] private Transform _parentForMoonItem;
        [SerializeField] private int _targetHamsterCount = 50;
        [SerializeField] private bool _rightIsFirst = false;
        [SerializeField] private bool _withSpace;
        [SerializeField] private bool _withMoon;
        [SerializeField] private Vector2 _zoomMinMax = Vector2.zero;

        private List<HamsterPyramidItem> _pyramidCollection = new List<HamsterPyramidItem>();
        private List<IPlacedObject> _placedObjects = new List<IPlacedObject>();

        private PlacedEmptySpace _space;
        private MoonPyramidItem _moonItem;
        private HamsterPyramidItem _targetHamster;
        private bool _isShowed;
        private bool _isInitialized;

        // private void Start()
        // {
        //     Initialize();
        // }

        private void OnEnable()
        {
            
        }

        public float Show(bool immediate = false, Action callBack = null)
        {
            if (!_isInitialized)
            {
                Initialize();
                if (!_isInitialized)
                {
                    callBack?.Invoke();
                    return 0f;
                }
            }
            
            _isShowed = true;
            gameObject.SetActive(true);
            // _cameraController.gameObject.SetActive(true);
            // _parentForHamsterItem.gameObject.SetActive(true);
            // _parentForMoonItem.gameObject.SetActive(true);
            BuildPyramid();
            SetupAnimationStates();
            InitializeCamera();
            // var bounds = _placedTool.GetBounds();
            // _cameraController.Initialize(bounds);
            if (!immediate)
                _cameraController.ZoomAt(_zoomMinMax.y, true);
            
            return _cameraController.FocusingAt(_targetHamster.HamsterPlacedObject.GetBound().center, _zoomMinMax.x, immediate, callBack);
            // return _cameraController.FocusingAt(_zoomMinMax.x, false, callBack);
            // _cameraController.ZoomAt(_zoomMinMax.y, true);
            // _cameraController.ZoomAt(_zoomMinMax.x, false, callBack);
        }

        public float Hide(bool immediate = false, Action callBack = null)
        {
            if (!_isInitialized)
            {
                callBack?.Invoke();
                return 0f;
            }
            
            _isShowed = false;
            // _parentForHamsterItem.gameObject.SetActive(false);
            // _parentForMoonItem.gameObject.SetActive(false);
            callBack += Dispose;
            callBack += () => gameObject.SetActive(false);
            return _cameraController.FocusingAt(_zoomMinMax.y, immediate, callBack);
            // _cameraController.ZoomAt(_zoomMinMax.y, false, callBack);
        }

        private void Initialize()
        {
            // _dataProvider = dataProvider;
            
            if(!_dataProvider.TryGetData(out var visualData)) return;
            
            
            // await UniTask.WaitUntil(() => _networkManager.IsAuthorized);
            
            // var result = await _networkManager.Piramid();

            // if (result == null || result.PyramidCollectionItems == null || result.PyramidCollectionItems.Length == 0)
            // {
                // Debug.LogError($"[{nameof(HamsterPyramidController)}] Pyramid collection is empty.");
                // return;
            // }
            
            // _collectionManager.SetupCollection(result.PyramidCollectionItems);


            if (_withMoon)
            {
                if (!_dataProvider.TryGetMoonData(out var moonInfo)) return;
                
                _moonItem = Instantiate(_moonPyramidPrefab, _parentForMoonItem);
                _moonItem.Init(moonInfo.BankAmount.ToString());
            }

            _isInitialized = true;
            
            foreach (var data in visualData)
            {
                var item = Instantiate(_hamsterPrefab, _parentForHamsterItem);
                item.Setup(data);
                _pyramidCollection.Add(item);
            }

            // var moonInfo = await _networkManager.GetMoonInfo();
            // _moonItem = Instantiate(_minePyramidPrefab, _parentForMoonItem);
            // _moonItem.Init(moonInfo.BankAmount.ToString());
            
            _dataProvider.OnAddHamster += AddHamster;
            _dataProvider.OnTargetChanged += SetTargetHamster;
            
            // BuildPyramid();
            // SetupAnimationStates();
            // InitializeCamera();
        }

        private void Dispose()
        {
            if(!_isInitialized) return;

            _isInitialized = false;

            _dataProvider.OnAddHamster -= AddHamster;
            _dataProvider.OnTargetChanged -= SetTargetHamster;
            
            foreach (var item in _pyramidCollection)
                Destroy(item.gameObject);
            
            _pyramidCollection.Clear();
            
            if(_withMoon && _moonItem != null) 
                Destroy(_moonItem.gameObject);
            
            _space = null;
            _placedObjects.Clear();
        }

        private void BuildPyramid()
        {
            _placedObjects.Clear();
            
            foreach (var item in _pyramidCollection)
                _placedObjects.Add(item.HamsterPlacedObject);

            if (_withSpace)
            {
                if (_placedObjects.Count < _targetHamsterCount)
                {
                    var dif = _targetHamsterCount - _placedObjects.Count;
                    var size = _hamsterPrefab.HamsterPlacedObject.Size;
                    size.y *= dif;
                
                    size += Vector3.one * _placedTool.Spacing * (dif - 1);

                    _space = new PlacedEmptySpace(size);
                    _placedObjects.Add(_space);
                }
            }

            _targetHamster = _pyramidCollection[_pyramidCollection.Count - 1];
            
            if(_withMoon && _moonItem != null) 
                _placedObjects.Add(_moonItem.PlacedObject);
            
            _placedTool.PlaceObjects(_placedObjects);
            UpdateBgRange();
        }

        private void InitializeCamera()
        {
            var bounds = _placedTool.GetBounds();

            var targetPointCamera = _targetHamster.HamsterPlacedObject.GetBound().center;
            _cameraController.Initialize(bounds, targetPointCamera);
        }

        public void ShowMoon(bool immediately = false, Action callBack = null)
        {
            if(!_withMoon || _moonItem == null) return;
            
            _cameraController.FocusingAt(_moonItem.PlacedObject.GetBound().center, immediately, callBack);
        }

        public void ShowTargetHamster(bool immediately = false, Action callBack = null)
        {
            if(_targetHamster == null) return;
            
            _cameraController.FocusingAt(_targetHamster.HamsterPlacedObject.GetBound().center, immediately, callBack);
        }
        
        private void AddHamster(IHamsterVisualData hamsterViewData)
        {
            // if (_pyramidCollection.Find(x => x.Id == hamsterViewData.Id))
            // {
            //     Debug.LogError($"[{nameof(HamsterPyramidController)}] New collection item is already exist in the map.");
            //     return;
            // }
            // _collectionManager.SetupCollectionItem(hamsterViewData);
            var item = Instantiate(_hamsterPrefab, _parentForHamsterItem);
            
            item.Setup(hamsterViewData);

            _pyramidCollection.Remove(_targetHamster);
            _pyramidCollection.Add(item);
            _pyramidCollection.Add(_targetHamster);
            
            if(!_isShowed) return;

            BuildPyramid();
            SetupAnimationStates();
            
            var bounds = _placedTool.GetBounds();
            _cameraController.UpdateBounds(bounds);
            _cameraController.FocusingAt(_targetHamster.HamsterPlacedObject.GetBound().center, true);
        }
       
        private void SetTargetHamster(IHamsterVisualData hamsterViewData)
        {
            // if(hamsterViewData.Id == _targetHamster.Id) return;
            
            var hamsterItem = _pyramidCollection.Find(x => x.Id == hamsterViewData.Id);
            if (hamsterItem == null)
            {
                Debug.LogError($"[{nameof(HamsterPyramidController)}] Target item is not exist in the map.");
                return;
            }

            var currentData = _targetHamster.VisualData;
            _targetHamster.Setup(hamsterItem.VisualData);
            hamsterItem.Setup(currentData);
            _targetHamster = hamsterItem;

            // _pyramidCollection.Remove(hamsterItem);
            // _pyramidCollection.Add(hamsterItem);
            
            if(!_isShowed) return;
            
            // BuildPyramid();
            // SetupAnimationStates();
            _cameraController.FocusingAt(_targetHamster.HamsterPlacedObject.GetBound().center, true);
        }

        private void SetupAnimationStates()
        {
            //if (_pyramidCollection.Count == 1)
            //{
            //    _pyramidCollection[0].SetState(_rightIsFirst ?  HamsterPyramidState.Right_finish : HamsterPyramidState.Left_finish);
            //    return;
            //}
            
            _pyramidCollection[0].SetState(HamsterPyramidState.Center);
            var isRightPlaced = !_rightIsFirst;
            
            for (int i = 1; i < _pyramidCollection.Count - 1; i++)
            {
                _pyramidCollection[i].SetState(isRightPlaced ? HamsterPyramidState.Right : HamsterPyramidState.Left);
                isRightPlaced = !isRightPlaced;
            }
            _pyramidCollection[_pyramidCollection.Count - 1].SetState(isRightPlaced ? HamsterPyramidState.Right_finish : HamsterPyramidState.Left_finish);
        }

        private void UpdateBgRange()
        {
            var bounds = _placedTool.GetBounds();
            var minValueY = bounds.min.y;
            var maxValueY = bounds.max.y;
            
            if (!_withSpace)
            {
                if (_placedObjects.Count < _targetHamsterCount)
                {
                    var dif = _targetHamsterCount - _placedObjects.Count;
                    var size = _hamsterPrefab.HamsterPlacedObject.Size;
                    size.y *= dif;
                
                    size += Vector3.one * _placedTool.Spacing * (dif - 1);
                    maxValueY += size.y;
                }
            }

            if (!_withMoon)
            {
                var sizeMoon = _moonPyramidPrefab.PlacedObject.GetBound();
                maxValueY += sizeMoon.size.y + _placedTool.Spacing;
            }
            
            _bgColorController.SetRange(minValueY, maxValueY);
        }

        // private void OnDisable()
        // {
        //     _dataProvider.OnAddHamster += AddHamster;
        //     _dataProvider.OnTargetChanged += SetTargetHamster;
        // }
//
// #if UNITY_EDITOR
//         public bool _isImmediate;
//         public bool _isCanCanceled;
//         
//         [ContextMenu(nameof(GoToTarget))]
//         public void GoToTarget()
//         {
//             if(_targetHamster == null) return;
//             
//             _cameraController.FocusingAt(_targetHamster.HamsterPlacedObject.GetBound().center, _isImmediate, null, _isCanCanceled);
//         }
// #endif
    }
}