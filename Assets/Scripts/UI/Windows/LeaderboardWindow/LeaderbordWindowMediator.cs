
using System;
using Cysharp.Threading.Tasks;
using DataModels.Leaderboard;
using DataModels.PlayerData;
using Managers;
using UnityEngine;
using Zenject;

namespace UI.Windows.LeaderboardWindow
{
    public class LeaderboardWindowMediator : BaseWindowMediator<LeaderboardWindowView, WindowData>
    {
        [Inject] private INetworkManager _networkManager;
        [Inject] private PlayerManager _playerManager;
        [Inject] private LeaderboardManager _leaderboardManager;
        
        
        protected override async void ShowStart()
        {
            base.ShowStart();
            await GetLeaderboard();
        }

        private async UniTask GetLeaderboard()
        {
           var leaderboardData = _leaderboardManager.LeaderboardData;

            for (var i = 0; i < leaderboardData.Length; i++)
            {
                if (_playerManager.PlayerCollection.ContainsKey(leaderboardData[i].equipment.collection_id))
                {
                    leaderboardData[i].equipment.leaderboard_sprite = _playerManager
                        .PlayerCollection[leaderboardData[i].equipment.collection_id].spriteIcon;
                }
                else
                {
                    leaderboardData[i].equipment.leaderboard_sprite = _playerManager
                        .CurrentCollectionItem.spriteIcon;
                }
                Target.InstantiateMarketItem().UpdateItemView(leaderboardData[i]);
            }
        }
    }
}