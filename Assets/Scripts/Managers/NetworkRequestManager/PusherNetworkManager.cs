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
using PusherClient;
using TMPro;
using UnityEngine;
using Channel = PusherClient.Channel;

namespace Managers
{
    
    public class PusherNetworkManager : INetworkManager
    {
        public string AccessToken { get; set; }

        private Pusher _pusher;
        private Channel _channel;
        private const string APP_KEY = "8d197f9cbeb0fa5df5c6";
        private const string APP_CLUSTER = "eu";

        public PusherNetworkManager()
        {
            Initialize();
        }
        
        private async void Initialize()
        {
            if (_pusher == null && (APP_KEY != "APP_KEY") && (APP_CLUSTER != "APP_CLUSTER"))
            {
                _pusher = new Pusher(APP_KEY, new PusherOptions()
                {
                    Cluster = APP_CLUSTER,
                    Encrypted = true
                });
        
                _pusher.Error += OnPusherOnError;
                _pusher.ConnectionStateChanged += PusherOnConnectionStateChanged;
                _pusher.Connected += PusherOnConnected;
                _channel = await _pusher.SubscribeAsync("my-channel");
                _pusher.Subscribed += OnChannelOnSubscribed;
                await _pusher.ConnectAsync();
            }
            else
            {
                Debug.LogError("APP_KEY and APP_CLUSTER must be correctly set. Find how to set it at https://dashboard.pusher.com");
            }
        }
        
        public async void Dispose()
        {
            if (_pusher != null)
            {
                await _pusher.DisconnectAsync();
            }
        }
        
        private void PusherOnConnected(object sender)
        {
            Debug.Log("Connected");
            _channel.Bind("my-event", (dynamic data) =>
            {
                Debug.Log("my-event received");
            });
        }

        private void PusherOnConnectionStateChanged(object sender, ConnectionState state)
        {
            Debug.Log("Connection state changed");
        }

        private void OnPusherOnError(object s, PusherException e)
        {
            Debug.Log("Errored");
        }

        private void OnChannelOnSubscribed(object s, Channel channel)
        {
            Debug.Log("Subscribed");
        }

        public bool IsAuthorized => false;

        public async UniTask<PlayerDataModel> Authorization(string authToken)
        {
            var result =  _channel.TriggerAsync("my-event", "test");
            // Debug.Log(result);
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
        public UniTask<bool> CanceGame(EndGameDataModel model)
        {
            return new UniTask<bool>();
        }
        

        public UniTask<StatisticSimpleData> GetStatistic()
        {
            return new UniTask<StatisticSimpleData>();
        }

        public  UniTask<PlayerResourcesData[]> UpdatePlayerBalances()
        {
            return new UniTask<PlayerResourcesData[]>();
        }

        public UniTask<MoonInfoData> GetMoonInfo()
        {
            return new UniTask<MoonInfoData>();
        }

        public UniTask<bool> FakeAddUserPoints(string email, int point_id, int value)
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