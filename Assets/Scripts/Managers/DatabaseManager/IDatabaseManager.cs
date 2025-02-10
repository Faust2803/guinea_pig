using System;
using System.Threading.Tasks;
using DataModels.PlayerData;

namespace Managers.DatabaseManager
{
    public interface IDatabaseManager
    {
        public Task Init();
        public void SaveUserData(string userId, PlayerDataModel userData);
        public void LoadUserData(string userId);
        public void DeleteUserData(string userId);
        
        public event Action<bool, string, PlayerDataModel> OnDataLoaded;
        public event Action<bool, string> OnDataSaved;
        public event Action<bool, string> OnDataDeleted;
    }
}