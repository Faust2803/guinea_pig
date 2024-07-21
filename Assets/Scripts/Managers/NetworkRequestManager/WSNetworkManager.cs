using Common.Statistic.DataDto;
using Configs;
using Cysharp.Threading.Tasks;
using DataModels.Achievement;
using DataModels.CollectionsData;
using DataModels.GameData;
using DataModels.MoonBank;
using DataModels.Leaderboard;
using DataModels.PlayerData;
using NativeWebSocket;
using UnityEngine;

namespace Managers
{
    public class WSNetworkManager: INetworkManager
    {
        public string AccessToken { get; set;}
        
        
        WebSocket websocket;
        
        public async void Initialize()
        {
            //websocket = new WebSocket(HamstaUri.sockedHost);
        
            websocket.OnOpen += () =>
            {
                Debug.Log("Connection open!");
            };
        
            websocket.OnError += (e) =>
            {
                Debug.Log("Error! " + e);
            };
        
            websocket.OnClose += (e) =>
            {
                Debug.Log("Connection closed!");
            };
        
            websocket.OnMessage += (bytes) =>
            {
                Debug.Log("OnMessage!");
                Debug.Log(bytes);
        
                // getting the message as a string
                // var message = System.Text.Encoding.UTF8.GetString(bytes);
                // Debug.Log("OnMessage! " + message);
            };
            // waiting for messages
            await websocket.Connect();
        }

        public async  void Dispose()
        {
            await websocket.Close();
        }

        public bool IsAuthorized => false;

        public async UniTask<PlayerDataModel> Authorization(string authToken)
        {
            if (websocket.State == WebSocketState.Open)
            {
                //JsonConvert.SerializeObject();
                   

                // Sending plain text
                await websocket.SendText(authToken);
            }

            return new PlayerDataModel();
        }
        
        public UniTask<PyramidCollectionData> Piramid()
        {
            return new UniTask<PyramidCollectionData>();
        }
        
        public UniTask<CollectionItemDataModel[]> Collection()
        {
            return new UniTask<CollectionItemDataModel[]>();
        }
        
        public UniTask<bool> Equip(int id)
        {
            return new UniTask<bool>();
        }

        public UniTask<CollectionItemDataModel[]> Market()
        {
            return new UniTask<CollectionItemDataModel[]>();
        }

        public UniTask<bool> MarketBuy(int id)
        {
            return new UniTask<bool>();
        }
        
        public UniTask<StartGameDataModel> StartGame()
        {
            return new UniTask<StartGameDataModel>();
        }

        public UniTask<bool> EndGame(EndGameDataModel model)
        {
            return new UniTask<bool>();
        }

        public UniTask<StatisticSimpleData> GetStatistic()
        {
            return new UniTask<StatisticSimpleData>();
        }
        public UniTask<bool> CanceGame(EndGameDataModel model)
        {
            return new UniTask<bool>();
        }

        public  UniTask<PlayerResourcesData[]> UpdatePlayerBalances()
        {
            return new UniTask<PlayerResourcesData[]>();
        }

        public UniTask<MoonInfoData> GetMoonInfo()
        {
            return new UniTask<MoonInfoData>();
        }

        public UniTask<bool> FakeAddUserPoints(string email,int point_id, int value)
        {
            return new UniTask<bool>();
        }

        public UniTask<bool> AddUserHP(int count, int collectionId)
        {
            return new UniTask<bool>();
        }

        public UniTask<LostUserHPResult> LostUserHP(int count, int collectionId)
        {
            return new UniTask<LostUserHPResult>();
        }
        
        public UniTask<LeaderboardUsers[]> GetLeaderboard(LeaderboardOrder order = LeaderboardOrder.collected_beans)
        {
            return new UniTask<LeaderboardUsers[]>();
        }
        
        public  UniTask<AchievementModel[]> GetAchievementList()
        {
            return new UniTask<AchievementModel[]>();
        }

        public UniTask<bool> AchievementTaskCompleat(int achievementId, int taskId, int count)
        {
            return new UniTask<bool>();
        }

        public UniTask<bool> AchievementCompleated(int achievementId)
        {
            return new UniTask<bool>();
        }
        
        public UniTask SendAnalytics(AnalyticDataModel model)
        {
            return new UniTask<bool>();
        }

        public UniTask<bool> RenameUser(string newNickname)
        {
            return new UniTask<bool>();
        }

        public UniTask<bool> ResetUser()
        {
            throw new System.NotImplementedException();
        }
    }
}