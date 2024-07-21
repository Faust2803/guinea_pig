using System;
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
using Zenject;

namespace Managers
{
    public interface INetworkManager:IDisposable
    {
        public bool IsAuthorized { get; }
       
        public UniTask<PlayerDataModel> Authorization(string authToken);
        public UniTask<PlayerResourcesData[]> UpdatePlayerBalances();
        public UniTask<PyramidCollectionData> Piramid();
        public UniTask<CollectionItemDataModel[]> Collection();
        public UniTask<bool> Equip(int id);
        public UniTask<CollectionItemDataModel[]> Market();
        public UniTask<bool> MarketBuy(int id);
        public UniTask<StartGameDataModel> StartGame();
        public UniTask<bool> EndGame(EndGameDataModel model);
        public UniTask<StatisticSimpleData> GetStatistic();
        public UniTask<bool> CanceGame(EndGameDataModel model);
        public UniTask<bool> AddUserHP(int count, int collectionId);
        public UniTask<LostUserHPResult> LostUserHP(int count, int collectionId);
        public UniTask<MoonInfoData> GetMoonInfo();
        public UniTask<LeaderboardUsers[]> GetLeaderboard(LeaderboardOrder order = LeaderboardOrder.collected_beans);
        public UniTask<AchievementModel[]> GetAchievementList();
        public UniTask<bool> AchievementTaskCompleat(int achievementId, int taskId, int count);
        public UniTask<bool> AchievementCompleated(int achievementId);
        public UniTask SendAnalytics(AnalyticDataModel model);
        public UniTask<bool> RenameUser(string newNickname);
        public UniTask<bool> ResetUser();

        //for testing
        public UniTask<bool> FakeAddUserPoints(string email, int point_id, int value);
    }
}