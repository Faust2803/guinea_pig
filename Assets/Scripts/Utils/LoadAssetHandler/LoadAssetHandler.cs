using System;
using System.Collections;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Object = UnityEngine.Object;

namespace Utils.LoadAssetHandler
{
    public interface ILoadAssetHandler<T> : IEnumerator where T : Object
    {
        event Action<T> OnCompleted;
        event Action OnFailed;
        UniTask<T> Task { get; }
        IDownloadAssetStatusHandler DownloadStatusHandler { get; }
        bool IsCompleted { get; }
        bool IsDone { get; }
        T Result { get; }
        void ReleaseInstance();
        void Release();
    }

    public class LoadAssetHandler<T> : ILoadAssetHandler<T> where T : Object
    {
        public event Action<T> OnCompleted;
        public event Action OnFailed;
        
        public bool IsCompleted => _isCompeted;
        public bool IsDone => _isDone;
        public UniTask<T> Task => WaitResult();
        public T Result => _result;
        public IDownloadAssetStatusHandler DownloadStatusHandler => _downloadHandler;
        
        private AsyncOperationHandle<T> _operation;
        private IEnumerator _operationNumerator;
        private object _reference;
        
        private bool _isCompeted;
        private bool _isDone;
        private T _result;
        private DownloadAssetStatusHandler _downloadHandler;
        
        public LoadAssetHandler(AssetReference reference)
        {
            _reference = reference;
            _result = default;
            _downloadHandler = new DownloadAssetStatusHandler();
            Handle().Forget();
        }
    
        public LoadAssetHandler(string key)
        {
            _reference = key;
            _result = default;
            _downloadHandler = new DownloadAssetStatusHandler();
            Handle().Forget();
        }

        private async UniTask<T> WaitResult()
        {
            while (!_isDone)
                await UniTask.Yield();

            return _result;
        }

        private async UniTaskVoid Handle()
        {
            var operationStatus = AsyncOperationStatus.None;
            var sizeLoading = 0L;

            try
            {
                var preloadSize = Addressables.GetDownloadSizeAsync(_reference);
                await UniTask.WaitUntil(() => !preloadSize.IsDone);
                sizeLoading = preloadSize.Result;
                operationStatus = preloadSize.Status;

                Addressables.Release(preloadSize);
            }
            catch (Exception e)
            {
                Debug.LogError($"[{GetType()}] Loading asset with type {typeof(T)} by reference {_reference} is failed with exception: {e}");
                _isDone = true;
                OnFailed?.Invoke();
                return;
            }
            
            if (operationStatus == AsyncOperationStatus.Succeeded && sizeLoading > 0)
            {
                Debug.Log(operationStatus);
                try
                {
                    var downloading = Addressables.DownloadDependenciesAsync(_reference);
                    _downloadHandler.Initialize(downloading);

                    await UniTask.WaitUntil(() => !_downloadHandler.CurrentDownloadingStatus.IsDone);

                    operationStatus = downloading.Status;
                    Addressables.Release(downloading);
                }
                catch (Exception e)
                {
                    Debug.LogError($"[{GetType()}] Loading asset with type {typeof(T)} by reference {_reference} is failed with exception: {e}");
                    _isDone = true;
                    OnFailed?.Invoke();
                    return;
                }
            }

            if (operationStatus != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"[{GetType()}] Downloading asset with type {typeof(T)} by reference {_reference} is failed with status: {operationStatus}.");
                _isDone = true;
                OnFailed?.Invoke();
                return;
            }

            var result = default(T);

            try
            {
                _operation = Addressables.LoadAssetAsync<T>(_reference);
                await UniTask.WaitUntil(() => !_operation.IsDone);
                result = _operation.Result;
            }
            catch (Exception e)
            {
                Debug.LogError($"[{GetType()}] Loading asset with type {typeof(T)} by reference {_reference} is failed with exception: {e}");

                _isDone = true;
                OnFailed?.Invoke();
                
                return;
            }

            _isDone = true;

            if (_operation.Status != AsyncOperationStatus.Succeeded)
            {
                Debug.LogError($"[{GetType()}] Loading asset with type {typeof(T)} by reference {_reference} is failed with status: {operationStatus}.");
                OnFailed?.Invoke();
                return;
            }

            _result = result;
            _isCompeted = true;

            OnCompleted?.Invoke(result);
        }

        public void ReleaseInstance()
        {
            if (_operation.IsValid())
                Addressables.ReleaseInstance(_operation);
        }

        public void Release()
        {
            if(_operation.IsValid())
                Addressables.Release(_operation);
        }
        
        public bool MoveNext()
        {
            return !_isDone;
        }

        public void Reset(){}

        public object Current => _result;
    }
}