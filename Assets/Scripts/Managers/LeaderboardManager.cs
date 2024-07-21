using Configs;
using Cysharp.Threading.Tasks;
using DataModels.Leaderboard;
using Zenject;

namespace Managers
{
    public class LeaderboardManager
    {
        [Inject] private INetworkManager _networkManager;
        public LeaderboardUsers[] LeaderboardData;
        public LeaderboardUsers MyLeaderboardData;
        
        public async UniTask GetLeaderboard(LeaderboardOrder order = LeaderboardOrder.collected_beans)
        {
            LeaderboardData   = await _networkManager.GetLeaderboard(order);
            for (var i = 0; i < LeaderboardData.Length; i++)
            {
                if (LeaderboardData[i].current_position == true)
                {
                    MyLeaderboardData = LeaderboardData[i];
                }
            }
            
            
        }
    }
}