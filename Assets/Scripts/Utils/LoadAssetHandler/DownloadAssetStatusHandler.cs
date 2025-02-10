using System;
using Cysharp.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Utils.LoadAssetHandler
{
    public interface IDownloadAssetStatusHandler
    {
        event Action<IDownloadAssetStatusHandler> OnStatusChanged;
        event Action<IDownloadAssetStatusHandler> OnFailed;
        event Action<IDownloadAssetStatusHandler> OnCompleted;
        DownloadStatus CurrentDownloadingStatus { get; }
        bool IsStarted { get; }
        bool IsCompleted { get; }
        bool IsFailed { get; }
    }

    public class DownloadAssetStatusHandler : IDownloadAssetStatusHandler
    {
        public event Action<IDownloadAssetStatusHandler> OnStatusChanged;
        public event Action<IDownloadAssetStatusHandler> OnFailed;
        public event Action<IDownloadAssetStatusHandler> OnCompleted;
    
        public DownloadStatus CurrentDownloadingStatus => _currentStatus;
        public bool IsStarted => _isStarted;
        public bool IsCompleted => _isCompleted;
        public bool IsFailed => _isFailed;
    
        private DownloadStatus _currentStatus;
        private bool _isStarted;
        private bool _isCompleted;
        private bool _isFailed;

        public void Initialize(AsyncOperationHandle operationHandle)
        {
            _isCompleted = false;
            _isFailed = false;

            if (!operationHandle.IsValid() || operationHandle.Status == AsyncOperationStatus.Failed)
            {
                _currentStatus.IsDone = true;
                _isFailed = true;
                OnFailed?.Invoke(this);
                return;
            }

            if (operationHandle.IsDone)
            {
                _currentStatus = operationHandle.GetDownloadStatus();
                _currentStatus.IsDone = true;
                OnCompleted?.Invoke(this);
                return;
            }

            Handle(operationHandle).Forget();
        }

        private async UniTaskVoid Handle(AsyncOperationHandle operationHandle)
        {
            _isStarted = true;

            while (!_currentStatus.IsDone)
            {
                if (operationHandle.Status == AsyncOperationStatus.Failed)
                {
                    _isFailed = true;
                    _currentStatus.IsDone = true;
                    OnFailed?.Invoke(this);
                    return;
                }
                _currentStatus = operationHandle.GetDownloadStatus();
            
                OnStatusChanged?.Invoke(this);
            
                await UniTask.Yield();
            }

            _isCompleted = true;
            OnCompleted?.Invoke(this);
        }
    }
}