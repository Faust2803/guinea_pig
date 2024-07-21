
using System;
using System.Runtime.InteropServices;
using DataModels.Achievement;
using DataModels.PlayerData;
using Managers;
using UnityEngine;
using Zenject;

namespace UI.Windows.WalletWindow
{
    public class WalletWindowMediator :BaseWindowMediator<WalletWindowView, WindowData>
    {
        [Inject] private PlayerManager _playerManager;
        [Inject] private INetworkManager _networkManager;
        [Inject] private AchievementManager _achievementManager;

#if UNITY_WEBGL && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void CloseWindow();
#endif

        protected override void ShowStart()
        {
            base.ShowStart();
            
           
            Target.AddMoney.onClick.AddListener(OnAddMoney);
            Target.BuyItem.onClick.AddListener(OnBuyItem);
            Target.EquipItem.onClick.AddListener(OnEquipItem);
            Target.ExitButton.onClick.AddListener(Exit);
            
            Target.Prachute.onClick.AddListener(OnPrachute);
            Target.ReactiveFart.onClick.AddListener(OnReactiveFart);
            Target.AchiveTermosphere.onClick.AddListener(OnAchiveTermosphere);
            Target.AchiveSpace.onClick.AddListener(OnAchiveSpace);
        }
        
        protected override void CloseStart()
        { 
            base.CloseStart();
            Target.AddMoney.onClick.RemoveListener(OnAddMoney);
            Target.BuyItem.onClick.RemoveListener(OnBuyItem);
            Target.EquipItem.onClick.RemoveListener(OnEquipItem);
            Target.ExitButton.onClick.RemoveListener(Exit);
            
            Target.Prachute.onClick.RemoveListener(OnPrachute);
            Target.ReactiveFart.onClick.RemoveListener(OnReactiveFart);
            Target.AchiveTermosphere.onClick.RemoveListener(OnAchiveTermosphere);
            Target.AchiveSpace.onClick.RemoveListener(OnAchiveSpace);
        }

        private async void OnAddMoney()
        {
            var result = await _networkManager.FakeAddUserPoints(_playerManager.PlayerEmail ,2, 100);
            if (result)
            {
                _playerManager.Resources[ResourcesType.GoldenBean].Value += 100;
            }

            Debug.Log($"Resources count {_playerManager.Resources[ResourcesType.GoldenBean].Value}");
            
        }
        
        private async void OnBuyItem()
        {
            var result = await _playerManager.BuyCollectionItemInMarket(Int32.Parse(Target.BuyItemField));
            Debug.Log($"ByCollectionItemInMarket == [{result}]");
        }
        
        private async void OnEquipItem()
        {
            var result = await _playerManager.EquipCollectionItem(Int32.Parse(Target.EquipItemField));
            Debug.Log($"EquipCollectionItem == [{result}]");
        }
        
        private async void OnPrachute()
        {
            var result = _achievementManager.CheckAchievementTarget(new AchievementTarget(TargetType.UseCorn, 10));
            Debug.Log($"OnPrachute == [{result}]");
        }
        
        private async void OnReactiveFart()
        {
            var result = _achievementManager.CheckAchievementTarget(new AchievementTarget(TargetType.UsePeas, 10));
            Debug.Log($"OnReactiveFart == [{result}]");
        }
        
        private async void OnAchiveTermosphere()
        {
            var result = _achievementManager.CheckAchievementTarget(new AchievementTarget(TargetType.Achieve5Zone, 1));
            Debug.Log($"OnAchiveTermosphere == [{result}]");
        }
        
        private async void OnAchiveSpace()
        {
            var result = _achievementManager.CheckAchievementTarget(new AchievementTarget(TargetType.Achieve4Zone, 1));
            Debug.Log($"OnAchiveSpace == [{result}]");
        }


        private void Exit()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
            Application.OpenURL("https://t.me/DevHamstamaniaMiniappBot");

            CloseWindow();
#endif
        }


    }
}