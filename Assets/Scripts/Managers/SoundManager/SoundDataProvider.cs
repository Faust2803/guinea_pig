using System;
using Cysharp.Threading.Tasks;
using Managers.SoundManager.Base;
using Managers.SoundManager.Data;
using Managers.SoundManager.Enums;
using UnityEngine.AddressableAssets;
using Utils.LoadAssetHandler;

namespace Managers.SoundManager
{
    public class SoundDataProvider : ISoundDataProvider, IDisposable
    {
        private AssetReferenceT<SoundConfig> _reference;
        private LoadAssetHandler<SoundConfig> _loadHandler;
        private SoundConfig _config;
        private bool _isLoadedData;

        public SoundDataProvider (AssetReferenceT<SoundConfig> reference)
        {
            _reference = reference;
            Initialize().Forget();
        }

        private async UniTaskVoid Initialize()
        {
            _loadHandler = new LoadAssetHandler<SoundConfig>(_reference);
            await _loadHandler.Task;

            _config = _loadHandler.Result;
            _isLoadedData = _loadHandler.IsCompleted;
            
            if(!_loadHandler.IsCompleted) return;
            
            _config.Initialize();
        }
        
        public bool TryGetData(SoundId id, out ISoundData data)
        {
            data = null;
            return _isLoadedData && _config.TryGetData(id, out data);
        }

        public bool TryGetData(string id, out ISoundData data)
        {
            data = null;
            return _isLoadedData && _config.TryGetData(id, out data);
        }

        public void Dispose()
        {
            _loadHandler?.ReleaseInstance();
        }
    }
}