using System;
using System.Threading.Tasks;
using DataModels.PlayerData;
using Firebase;
using Firebase.Database;
using Newtonsoft.Json;
using UnityEngine;
using Firebase.Extensions;

namespace Managers.DatabaseManager
{
    public class FirebaseDatabaseManager : IDatabaseManager
    {
        private const string DB_URL = "https://game-portal-3d618-default-rtdb.europe-west1.firebasedatabase.app/";
        private const string USER_TABLE_KEY = "users";    
        public event Action<bool, string, PlayerDataModel> OnDataLoaded;
        public event Action<bool, string> OnDataSaved;
        public event Action<bool, string> OnDataDeleted;
        
        private DatabaseReference _dbReference;
        //private FirebaseAuth auth;
        

        public async Task Init()
        {
            // var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            // if (dependencyStatus == DependencyStatus.Available)
            // {
            //     FirebaseApp app = FirebaseApp.DefaultInstance;
            //
            //     // Получаем базу данных с URL
            //     _dbReference = FirebaseDatabase.GetInstance(app, DB_URL).RootReference;
            //     Debug.Log("Firebase подключен!");
            // }
            // else
            // {
            //     Debug.LogError($"Firebase не инициализирован: {dependencyStatus}");
            // }
            
            FirebaseApp app = FirebaseApp.DefaultInstance;
            // Получаем базу данных с URL
            _dbReference = FirebaseDatabase.GetInstance(app, DB_URL).RootReference;
        }
        
        public void SaveUserData(string userId, PlayerDataModel userData)
        {
            var json = JsonConvert.SerializeObject(userData);
            _dbReference.Child(USER_TABLE_KEY).Child(userId).SetRawJsonValueAsync(json).ContinueWithOnMainThread(task => 
            {
                if (task.IsCompleted)
                {
                    OnDataSaved?.Invoke(true, $"Данные сохранены! id = {userId}");
                }
                else
                {
                    OnDataSaved?.Invoke(false, $"Ошибка записи данных: {task.Exception}");
                }
            });
        }
        
        public void LoadUserData(string userId)
        {
            _dbReference.Child(USER_TABLE_KEY).Child(userId).GetValueAsync().ContinueWithOnMainThread(task =>
            {
                if (task.IsFaulted || task.Exception != null)
                {
                    OnDataLoaded?.Invoke(false, $"Ошибка загрузки данных: {task.Exception.Message}", null);
                    return;
                }
                if (task.IsCompleted && task.Result.Exists)
                {
                    var dataModel = JsonConvert.DeserializeObject<PlayerDataModel>(task.Result.GetRawJsonValue());
                    OnDataLoaded?.Invoke(true, userId, dataModel);
                }
                else
                {
                    OnDataLoaded?.Invoke(false, $"Пользователь не найден! id = {userId}", null);
                }
            });
        }

        public void DeleteUserData(string userId)
        {
            _dbReference.Child(USER_TABLE_KEY).Child(userId).RemoveValueAsync().ContinueWithOnMainThread(task => 
            {
                if (task.IsCompleted)
                {
                    OnDataDeleted?.Invoke(true, $"Данные удалены! id = {userId}");
                }
                else
                {
                    OnDataDeleted?.Invoke(false, $"Ошибка удаления данных:  {task.Exception}");
                }
            });
        }
    }
}