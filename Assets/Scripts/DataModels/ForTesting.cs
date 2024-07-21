using System;
using DataModels.CollectionsData;
using DataModels.PlayerData;
using UnityEngine.Serialization;

namespace DataModels
{
    [Serializable]
    public class ForTesting
    {
        public string email;
    }
    
    [Serializable]
    public class FakeAddUserPoints
    {
        public int point_id;
        public int value;
        public string email;
    }
    
    
}