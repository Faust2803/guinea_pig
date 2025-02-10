
using System;
using System.Threading.Tasks;
using DataModels.PlayerData;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;
using Newtonsoft.Json;

namespace Managers.ConfigDataManager
{
    public class FirebaseConfigDataManager:IConfigDataManager
    {
        private const string KEY = "gameConfig";
        
        public ConfigData ConfigData { get; private set; }
    
        public event Action OnGetConfig;
        
       
        public async Task Init()
        {
            var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
            if (dependencyStatus == DependencyStatus.Available)
            {
                Debug.Log("Firebase успешно инициализирован!");
                await FetchRemoteConfig();
            }
            else
            {
                Debug.LogError($"Ошибка Firebase: {dependencyStatus}");
            }
        }

        public async Task FetchRemoteConfig()
        {
            
            // Настраиваем интервал обновления (в секундах)
            FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(fetchTask =>
            {
                if (fetchTask.IsCompleted)
                {
                    Debug.Log("Удалённые параметры загружены!");
                    FirebaseRemoteConfig.DefaultInstance.ActivateAsync();
                    
                    var configData = FirebaseRemoteConfig.DefaultInstance.GetValue(KEY).StringValue;
                    ConfigData = JsonConvert.DeserializeObject<ConfigData>(configData);
                    OnGetConfig?.Invoke();
                }
                else
                {
                    Debug.LogError("Ошибка загрузки Remote Config: " + fetchTask.Exception);
                }
            });
        }

        
        
    }

    
}