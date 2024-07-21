using System;
using DataModels.CollectionsData;
using DataModels.PlayerData;
using UnityEngine.Serialization;

namespace DataModels
{
    [Serializable]
    public class AuthDataModel
    {
        public bool success;
        public PlayerDataModel player_data;
    }
    
    
}