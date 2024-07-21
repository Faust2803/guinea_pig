using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Cysharp.Threading.Tasks;
using DataModels.CollectionsData;
using DataModels.PlayerData;
using UI.Windows;
using UI.Windows.SimpleDialogWindow;
using Zenject;

namespace Managers
{
    public class PlayerManager
    {
        public const int Feed_Hamster_exchange_Rate = 10;
        public event Action<CollectionItemDataModel> OnBuyCollectionItem;
        public event Action<CollectionItemDataModel> OnEquipedCollectionItem;
        
        [Inject] private CollectionManager _collectionManager;
        [Inject] private INetworkManager _networkManager;
        [Inject] private UiManager _uiManager;
        [Inject] private LeaderboardManager _leaderboardManager;

        private Dictionary<int, CollectionItemDataModel> _playerCollection = new Dictionary<int, CollectionItemDataModel>();
        
        private Dictionary<int, CollectionItemDataModel> _marketCollection = new Dictionary<int, CollectionItemDataModel>();
        
        private Dictionary<ResourcesType, PlayerResourcesData> _resources = new Dictionary<ResourcesType, PlayerResourcesData>();

        public Dictionary<ResourcesType, PlayerResourcesData> Resources => _resources;
        public Dictionary<int, CollectionItemDataModel> PlayerCollection => _playerCollection;
        public Dictionary<int, CollectionItemDataModel> MarketCollection => _marketCollection;
        public CollectionItemDataModel CurrentCollectionItem  { get; private set; }
        public string PlayerEmail  { get; private set; }
        public string PlayerName   { get; private set; }
        
       

        public async UniTask SetUpAllPlayerData(PlayerDataModel playerDataModel)
        {
            PlayerEmail = playerDataModel.email;
            PlayerName = playerDataModel.name;
            
            SetUpPlayerResources( playerDataModel.balances);
            
            SetUpPlayerCollection(await _networkManager.Collection());
            SetUpStoreItem(await _networkManager.Market());
        }
        
        public async UniTask UpdatePlayerData()
        {
            var result = await _networkManager.UpdatePlayerBalances();
            UpdatePlayerResources(result);
            SetUpPlayerCollection(await _networkManager.Collection());
        }
        
        public async UniTask<CollectionItemDataModel> BuyCollectionItemInMarket(int unity_id)
        {
            if (_marketCollection.ContainsKey(unity_id) && _marketCollection[unity_id].collection_status == 0)
            {
                if (!CheckResources(
                        (ResourcesType)_marketCollection[unity_id].collection_cost_type,
                        _marketCollection[unity_id].collection_cost)
                    )
                {
                    var windowData = new SimpleDialogWindowData
                    {
                        Title = "Oops!",
                        Description = "Go earn more coins.",
                        NoButtonActive = false,
                    };
                    _uiManager.ForceOpenWindow(WindowType.SimpleDialogWindow, windowData);
                    return null;
                }
                
                var result = await _networkManager.MarketBuy(unity_id);
                if (result)
                {
                    _marketCollection[unity_id].collection_status = 1;

                    if (_playerCollection.ContainsKey(unity_id))
                    {
                        _playerCollection[unity_id].collection_status = 1;                       
                    }
                    else
                    {
                        _playerCollection.Add(unity_id,  _marketCollection[unity_id]);
                    }

                    _playerCollection[unity_id].collection_current_hp = _playerCollection[unity_id].collection_hp;

                    ChangeResourceBalance(
                        (ResourcesType)_marketCollection[unity_id].collection_cost_type,
                        -_marketCollection[unity_id].collection_cost
                        );

                    
                    OnBuyCollectionItem?.Invoke(_playerCollection[unity_id]);
					return _marketCollection[unity_id];
                }
            }
            return null;
        }

        public async UniTask<bool> EquipCollectionItem(int unity_id)
        {
            if (_playerCollection.ContainsKey(unity_id) && CurrentCollectionItem.collection_id != unity_id)
            {
                var result = await _networkManager.Equip(unity_id);
                if (result)
                {
                    CurrentCollectionItem.collection_equiped = 0;
                    CurrentCollectionItem = _playerCollection[unity_id];
                    _playerCollection[unity_id].collection_equiped = 1;
                    
                    OnEquipedCollectionItem?.Invoke(_playerCollection[unity_id]);
                }
                return result;
            }
            return false;
        }

        public async UniTask<LostUserHPResult> LostHp(int id = -1, int hp = 1)
        {
            if (id == -1)
            {
                if (CurrentCollectionItem.collection_current_hp - hp >= 0)
                {
                    var result = await _networkManager.LostUserHP(hp, CurrentCollectionItem.collection_id);
                    if (result.success)
                    {
                        //костыль hp не може падати нижче 1
                        if (CurrentCollectionItem.collection_current_hp - hp < 1)
                        {
                            CurrentCollectionItem.collection_current_hp = 1;
                        }
                        else
                        {
                            CurrentCollectionItem.collection_current_hp -= hp;
                        }
                        return result;
                    }
                }
            }
            else
            {
                if (PlayerCollection.ContainsKey(id) &&
                    PlayerCollection[id].collection_current_hp - hp >= 0)
                {
                    var result = await _networkManager.LostUserHP(hp, CurrentCollectionItem.collection_id);
                    if (result.success)
                    {
                        //костыль hp не може падати нижче 1
                        if (CurrentCollectionItem.collection_current_hp - hp < 1)
                        {
                            CurrentCollectionItem.collection_current_hp = 1;
                        }
                        else
                        {
                            CurrentCollectionItem.collection_current_hp -= hp;
                        }
                        return result;
                    }
                }
            }
            return null;
        }

        public async UniTask<bool> FeedHamster(int hp = 1, int id = -1)
        {
            var exchangeRate = Feed_Hamster_exchange_Rate*hp;
            // if (CheckResources(ResourcesType.GoldenBean, exchangeRate))
            // {
                if (id == -1)
                {
                    if (CurrentCollectionItem.collection_current_hp + hp <= CurrentCollectionItem.collection_hp)
                    {
                        if (await _networkManager.AddUserHP(hp, CurrentCollectionItem.collection_id))
                        {
                            CurrentCollectionItem.collection_current_hp += hp;
                            //ChangeResourceBalance(ResourcesType.GoldenBean, - exchangeRate);
                            return true;
                        }
                    }
                }
                else
                {
                    if (PlayerCollection.ContainsKey(id) &&
                        PlayerCollection[id].collection_current_hp + hp <= PlayerCollection[id].collection_hp)
                    {
                        if (await _networkManager.AddUserHP(hp,  PlayerCollection[id].collection_id))
                        {
                            PlayerCollection[id].collection_current_hp += hp;
                            //ChangeResourceBalance(ResourcesType.GoldenBean, -exchangeRate);
                            return true;
                        }
                    }
                }
            // }
            // else
            // {
            //     var windowData = new SimpleDialogWindowData
            //     {
            //         Title = "Error",
            //         Description = "You don't have enough resources, try to earn more resources.",
            //         NoButtonActive = false,
            //     };
            //     _uiManager.ForceOpenWindow(WindowType.SimpleDialogWindow, windowData);
            // }
            return false;
        }

        private void SetUpPlayerResources(PlayerResourcesData[] balances)
        {
            _resources.Clear();
            _resources = balances.ToDictionary(item => item.ResourceType, item => item);
        }
        
        private void UpdatePlayerResources(PlayerResourcesData[] balances)
        {
            for (var i = 0; i < balances.Length; i++)
            {
                _resources[balances[i].ResourceType].Value = balances[i].Value;
            }
        }
        
        private void SetUpPlayerCollection(CollectionItemDataModel[] collection)
        {
            if (collection == null || collection.Length == 0)
            {
                return;
            }
            _playerCollection.Clear();
            _collectionManager.SetUpCollection(collection);
            _playerCollection = collection.ToDictionary(item => item.collection_id, item => item);

            for (var i = 0; i < collection.Length; i++)
            {
                if (collection[i].collection_equiped == 1)
                {
                    CurrentCollectionItem = collection[i];
                }
            }
        }
        
        private void SetUpStoreItem(CollectionItemDataModel[] collection)
        {
            if (collection == null || collection.Length == 0)
            {
                return;
            }
            _marketCollection.Clear();
            _collectionManager.SetUpCollection(collection);
            _marketCollection = collection.ToDictionary(item => item.collection_id, item => item);
        }

        private bool CheckResources(ResourcesType type, int value)
        {
            return _resources[type].Value >= value;
        }

        private void ChangeResourceBalance(ResourcesType type, int value)
        {
            _resources[type].Value += value;
        }

        public async UniTask<bool> RenameUser(string newNickname)
        {
            var result = await _networkManager.RenameUser(newNickname);
            if (result)
            {
                PlayerName = newNickname;
                await _leaderboardManager.GetLeaderboard();
            }

            return result;
        }

        public async UniTask<bool> ResetUser()
        {
            var result = await _networkManager.ResetUser();
            return result;
        }
    }
}