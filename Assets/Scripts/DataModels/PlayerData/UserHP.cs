using System;

namespace DataModels.PlayerData
{
    [Serializable]
    public class LostUserHPResult
    {
        public bool success;
        //public string message;
        public int moon_bank;
    }
    
    [Serializable]
    public class UserHP
    {
        //public int collection_id;
        public int count;
    }
}