using System;
using UnityEngine.Serialization;

namespace DataModels.Rewards
{
    [Serializable]
    public class RewardDataModel
    {
        public string name;
        public RewardType reward_type;
        public int unity_id;
        public int reward_count;
        public int collection_id;
    }
    
    
    public enum  RewardType
    {
        Collection,
        Currency
    }
}