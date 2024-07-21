using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Events;

namespace Managers
{
    public class AdressableLoaderManager
    {
        public UnityAction<float, float, float> OnLoadingProgress;
        public UnityAction OnLoadingDataComplite;

        public async UniTask LoadAdressabless(AssetLabel assetLabel)
        {
            var preloadSizeOp = Addressables.GetDownloadSizeAsync(assetLabel.ToString());
            var result = await preloadSizeOp;
            var preloadSize = result;
            Addressables.Release(preloadSizeOp);

            if (preloadSize > 0f)
            {
                var preloadOp = Addressables.DownloadDependenciesAsync(assetLabel.ToString());
                while (!preloadOp.IsDone)
                {
                    var status = preloadOp.GetDownloadStatus();
                    OnLoadingProgress?.Invoke(status.Percent, ConvertBytesToMegabytes(status.DownloadedBytes),
                        ConvertBytesToMegabytes(status.TotalBytes));
                    await UniTask.Delay(500);
                }
                Addressables.Release(preloadOp);
            }
            OnLoadingDataComplite?.Invoke();
        }

        private float ConvertBytesToMegabytes(long bytes, int digits = 2)
        {
            var result = (bytes / 1024f) / 1024f;
            return (float)System.Math.Round(result, digits);
        }
    }
    
    public enum AssetLabel
    {
        Main
    }
}
