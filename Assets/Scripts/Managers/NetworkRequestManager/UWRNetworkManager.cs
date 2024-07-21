using System;
using System.Text;
using Common.Statistic.DataDto;
using Configs;
using Cysharp.Threading.Tasks;
using DataModels;
using DataModels.Achievement;
using DataModels.CollectionsData;
using DataModels.GameData;
using DataModels.MoonBank;
using DataModels.Leaderboard;
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
        private string AccessToken { get; set;}
        public bool IsAuthorized => _isAuthorized;
        private bool _isAuthorized;
        [Inject] private UiManager _uiManager;
        
        public async UniTask<PlayerDataModel> Authorization(string authToken)
        {
            AccessToken = authToken;
            var result = await SendRequest<AuthDataModel>(HamstaUri.Authorization,
               null,
                RequestType.Post
                );
            Debug.Log($"Authorization result.success == [{result.success}]");
            if (result!= null && result.success)
            {
                AccessToken = result.player_data.access_token;
                _isAuthorized = true;
                Session();
                return result.player_data;
            }

            AccessToken = string.Empty;
            return null;
        }
        
        public async UniTask<PlayerResourcesData[]> UpdatePlayerBalances()
        {
            var result = await SendRequest<PlayerBalancesData>(HamstaUri.Balances, null, RequestType.Post);
            if (result.success)
            {
                return result.balances;
            }

            return null;
        }

        public async UniTask<PyramidCollectionData> Piramid()
        {
            // Debug.LogError($"Piramide request");
            var result = await SendRequest<PyramidCollectionData>(HamstaUri.Piramid);
            // return await SendRequest<CollectionItemDataModel[]>(HamstaUri.Piramid);
            var count = result == null ? "null" : 
                             result.PyramidCollectionItems == null ? "null" : result.PyramidCollectionItems.Length.ToString();
            // Debug.LogError($"Piramide responce: result count: {count}");
            return result;
            return await SendRequest<PyramidCollectionData>(HamstaUri.Piramid);
        }

        public async UniTask<CollectionItemDataModel[]> Collection()
        {
            var result = await SendRequest<CollectionDataModel>(HamstaUri.Collection, null, RequestType.Post);
            return result.success ? result.player_data : null;
        }
        
        public async UniTask<bool> Equip(int id)
        {
            var result =  await SendRequest<ConfirmationDataModel>(HamstaUri.Equip,
                JsonConvert.SerializeObject(
                    new EquipDataModel()
                    {
                       collection_id = id
                    }
                ),
                RequestType.Post);
            return result.success;
        }
        
        public async UniTask<CollectionItemDataModel[]> Market()
        {
            var result = await SendRequest<MarcetDataModel>(HamstaUri.Market);
            return result.market;
        }

        public async UniTask<bool> MarketBuy(int id)
        {
            var result =  await SendRequest<ConfirmationDataModel>(HamstaUri.MarketBuy,
                JsonConvert.SerializeObject(
                    new EquipDataModel()
                    {
                        collection_id = id
                    }
                ),
                RequestType.Post);
            return result.success;
        }

        public async UniTask<MoonInfoData> GetMoonInfo()
        {
            return await SendRequest<MoonInfoData>(HamstaUri.MoonInfo);
        }

        public async UniTask<bool> FakeAddUserPoints(string email, int point_id, int value)
        {
            var result =  await SendRequest<ConfirmationDataModel>(HamstaUri.FakeAddUserPoints,
                JsonConvert.SerializeObject(
                    new FakeAddUserPoints()
                    {
                        point_id = point_id,
                        value = value,
                        email = email
                    }
                ),
                RequestType.Post);
            return result.success;
        }

        public async UniTask<StartGameDataModel> StartGame()
        {
            var result =  await SendRequest<StartGameDataModel>(HamstaUri.StartGame);
            return result;
        }

        public async UniTask<bool> EndGame(EndGameDataModel model)
        {
            var result =  await SendRequest<ConfirmationDataModel>(HamstaUri.EndGame,
                JsonConvert.SerializeObject(model),
                RequestType.Post);
            return result.success;
        }
        
        public async UniTask<bool> CanceGame(EndGameDataModel model)
        {
            var result =  await SendRequest<ConfirmationDataModel>(HamstaUri.CancelGame,
                JsonConvert.SerializeObject(model),
                RequestType.Post);
            return result.success;
        }

        public async UniTask<StatisticSimpleData> GetStatistic()
        {
            var result = await SendRequest<StatisticSimpleData>(HamstaUri.GetStatistic);
            Debug.Log(result == null);
            return result;
        }
        
        public async UniTask<bool> AddUserHP(int count, int collectionId)
        {
            var result =  await SendRequest<ConfirmationDataModel>(HamstaUri.AddUserHP,
                JsonConvert.SerializeObject(
                    new UserHP()
                    {
                        //collection_id = collectionId,
                        count = count
                    }
                ),
                RequestType.Post);
            return result.success;
        }

        public async UniTask<LostUserHPResult> LostUserHP(int count, int collectionId)
        {
            var result =  await SendRequest<LostUserHPResult>(HamstaUri.LostUserHP,
                JsonConvert.SerializeObject(
                    new UserHP()
                    {
                        //collection_id = collectionId,
                        count = count
                    }
                ),
                RequestType.Post);
            result.success = true;

            return result;
        }
        
        public async UniTask<LeaderboardUsers[]> GetLeaderboard(LeaderboardOrder order = LeaderboardOrder.collected_beans)
        {
            var result =  await SendRequest<LeaderboardData>($"{HamstaUri.Leaderboard}/{order.ToString()}");
            return result.data;
        }

        public async UniTask<AchievementModel[]> GetAchievementList()
        {
            var result =  await SendRequest<AchievementDataModel>(HamstaUri.AchievementGetList);
            return result?.data;
        }

        public async UniTask<bool> AchievementTaskCompleat(int achievementId, int taskId, int count)
        {
            var result =  await SendRequest<ConfirmationDataModel>(HamstaUri.AchievementTaskCompleat,
                JsonConvert.SerializeObject(
                    new TaskCompleatedModel()
                    {
                       task_id = taskId,
                       achivement_id = achievementId,
                       counter = count
                    }
                ),
                RequestType.Post);
            return result.success;
        }

        public async UniTask<bool> AchievementCompleated(int achievementId)
        {
            var result =  await SendRequest<ConfirmationDataModel>(HamstaUri.AchievementCompleat,
                JsonConvert.SerializeObject(
                    new AchievementCompleatedModel()
                    {
                        achivement_id = achievementId
                    }
                ),
                RequestType.Post);
            return result.success;
        }

        public async UniTask SendAnalytics(AnalyticDataModel model)
        {
            var result =  await SendRequest<AnalyticsConfirmationDataModel>(HamstaUri.SendAnalytics,
                JsonConvert.SerializeObject(
                    model
                ),
                RequestType.Post);
            Debug.Log(result.success);
        }
        
        private async void Session()
        {
            while (true)
            {
                var result =  await SendRequest<ConfirmationDataModel>(HamstaUri.Session, null, RequestType.Post);
                Debug.Log(result.success);
                await UniTask.Delay(50000);
            }
            
        }

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
                request.SetRequestHeader("Authorization", $"Bearer {AccessToken}");
                
                await request.SendWebRequest();
            }
            catch (Exception e)
            {
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
        
        public async void Dispose()
        {
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

        public async UniTask<bool> RenameUser(string newNickname)
        {
            var result = await SendRequest<ConfirmationDataModel>(HamstaUri.UserRename,
                JsonConvert.SerializeObject(new PlayerNameDataModel
                { 
                    nickname = newNickname
                }),
                RequestType.Post);
            return result.success;
        }

        public async UniTask<bool> ResetUser()
        {
            var result = await SendRequest<ConfirmationDataModel>(HamstaUri.UserReset);
            return result.success;
        }
    }

    
}