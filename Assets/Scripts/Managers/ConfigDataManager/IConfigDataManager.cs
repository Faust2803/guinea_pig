using System;
using System.Threading.Tasks;
using DataModels.PlayerData;

namespace Managers.ConfigDataManager
{
    public interface IConfigDataManager
    {
        public ConfigData ConfigData{get;}

        public Task Init();
        public Task FetchRemoteConfig();
        
        public event Action OnGetConfig;
    }
}