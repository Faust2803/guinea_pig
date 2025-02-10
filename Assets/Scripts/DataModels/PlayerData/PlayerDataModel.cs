using System;


namespace DataModels.PlayerData
{
    [Serializable]
    public class PlayerDataModel
    {
        public string name;
        public string email;
        public int money;
        public string id;
        public int recoveryTime;
        public int level;
        public bool isNewUser;
        public int life;
    }

    
}
