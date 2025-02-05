using System;
using System.Text;
using Cysharp.Threading.Tasks;
using DataModels.PlayerData;
using Newtonsoft.Json;
using UI.Windows;
using UI.Windows.SimpleDialogWindow;
using UnityEngine;
using UnityEngine.Networking;
using Zenject;

namespace Managers
{
    public class UWRNetworkManager:INetworkManager
    {

        [Inject] private UiManager _uiManager;
        
        // public async UniTask<PlayerDataModel> Authorization(string authToken)
        // {
        //     /*var result = await SendRequest<AuthDataModel>(HamstaUri.Authorization,
        //        null,
        //         RequestType.Post
        //         );
        //     Debug.Log($"Authorization result.success == [{result.success}]");
        //     if (result!= null && result.success)
        //     {
        //        
        //         return result.player_data;
        //     }*/
        //     return null;
        // }
        
        

        private async UniTask<T> SendRequest<T>(string uri, string data = null, RequestType type = RequestType.Get)
        {
            uri += $"?v={DateTime.Now}";
            UnityWebRequest request = null;
            try
            {
                switch (type)
                {
                    case RequestType.Get:
                        request = UnityWebRequest.Get(uri);
                        break;
                    case RequestType.Delete:
                        request = UnityWebRequest.Delete(uri);
                        request.downloadHandler = new DownloadHandlerBuffer();
                        break;
                    case RequestType.Post:
                        request = new UnityWebRequest(uri, UnityWebRequest.kHttpVerbPOST);
                        request.downloadHandler = new DownloadHandlerBuffer();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(type), type, null);
                }
                
                if (!string.IsNullOrEmpty(data))
                {
                    request.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(data));
                }

                request.SetRequestHeader("Content-Type", "application/json");
                //request.SetRequestHeader("Authorization", $"Bearer {AccessToken}");
                
                await request.SendWebRequest();
            }
            catch (Exception e)
            {
                Debug.Log(e);
                ShowDisconnectWindow();
                return default(T);

            }
            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                ShowDisconnectWindow();
                return default(T);
            }
            if (request.result == UnityWebRequest.Result.Success)
            {
                Debug.Log(request.downloadHandler.text);
                return JsonConvert.DeserializeObject<T>(request.downloadHandler.text);
            }
            return default(T);
        }
        
        private enum RequestType
        {
            Post,
            Get,
            Delete
        }

        private void ShowDisconnectWindow()
        {
            var windowData = new SimpleDialogWindowData
            {
                Title = "Enternet Error",
                Description = "An error has occurred, check your internet connection and restart the game",
                NoButtonActive = false,
            };
            _uiManager.ForceOpenWindow(WindowType.SimpleDialogWindow, windowData);
        }
    }

    
}