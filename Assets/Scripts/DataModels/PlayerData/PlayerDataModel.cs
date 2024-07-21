using System;
using DataModels.CollectionsData;

namespace DataModels.PlayerData
{
    [Serializable]
    public class PlayerDataModel
    {
        public string access_token;
        public string refresh_token;
        public string name;
        public string email;
        
        
        public PlayerResourcesData[] balances;
        //public CollectionItemDataModel[] equipment;
        // public int total_beans;
        // public int total_games;
    }

    [Serializable]
    public class PlayerNameDataModel
    {
        public string nickname;
    }
}
