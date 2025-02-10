using System;
using System.Threading.Tasks;
using DataModels.PlayerData;
using Managers.ConfigDataManager;
using Managers.DatabaseManager;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class PlayerManager : IDisposable
    {
        
        [Inject] private UiManager _uiManager;
        [Inject] private IConfigDataManager _configDataManager;
        [Inject] private IDatabaseManager _databaseManager;

        private const string USER_ID = "userId";
        
        public event Action<bool> OnDataLoaded;
        public event Action<bool> OnDataSaved;
        public event Action<bool> OnUserDeleted;
        
        public PlayerDataModel DataModel  { get; private set; }
        
        private string _userId;
        
        public void Init()
        {
            DataModel = new PlayerDataModel();
            _databaseManager.OnDataLoaded += GetedUser;
            _databaseManager.OnDataSaved += SavedUser;
            _databaseManager.OnDataDeleted += DeletedUser;
            if (PlayerPrefs.HasKey(USER_ID))
            {
                _userId = PlayerPrefs.GetString(USER_ID);
                DataModel.id = _userId;
                DataModel.isNewUser = false;
            }
            else
            {
                DataModel.isNewUser = true;
            }
        }

        public void SignIn(string userId)
        {
            _userId = userId;
            PlayerPrefs.SetString(USER_ID, userId);
            PlayerPrefs.Save();
            TryGetUserData();
        }
        
        private void TryGetUserData()
        {
            _databaseManager.LoadUserData(_userId);
        }
        
        private void TrySaveUserData()
        {
            _databaseManager.SaveUserData(_userId, DataModel);
        }

        private void GetedUser(bool result, string message, PlayerDataModel playerData)
        {
            if (!result)
            {
               Debug.Log(message);

               if (DataModel.isNewUser)
               {
                   CreateNewUser();
                   return;
               }
            }
            else
            {
                DataModel = playerData;
            }
            
            OnDataLoaded?.Invoke(result);
        }
        
        private void SavedUser(bool result, string message)
        {
            if (!result)
            {
                Debug.Log(message); 
            }
            
            OnDataSaved?.Invoke(result);
        }
        
        private void DeletedUser(bool result, string message)
        {
            if (!result)
            {
                Debug.Log(message); 
            }
            
            OnUserDeleted?.Invoke(result);
        }

        private void CreateNewUser()
        {
            DataModel.isNewUser = false;
            DataModel.id = _userId;
            DataModel.level = _configDataManager.ConfigData.startLevel;
            DataModel.recoveryTime = _configDataManager.ConfigData.startRecoveryTime;
            DataModel.money = _configDataManager.ConfigData.startMoney;
            DataModel.name = _configDataManager.ConfigData.startName;
            DataModel.life = _configDataManager.ConfigData.StartLife;
            
            TrySaveUserData();
        }
        
        public void SignOut()
        {
            _uiManager = null;
            DataModel = new PlayerDataModel();
            DataModel.id = _userId;
            DataModel.isNewUser = true;
            PlayerPrefs.DeleteAll();
        }

        public void Dispose()
        {
            _databaseManager.OnDataLoaded -= GetedUser;
            _databaseManager.OnDataSaved -= SavedUser;
            _databaseManager.OnDataDeleted -= DeletedUser;
        }
    }
}