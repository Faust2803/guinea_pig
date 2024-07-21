using System;

namespace DataModels.PlayerData
{
    [Serializable]
    public class PlayerResourcesData
    {
        public int resources_value;
        public string resources_type;
        public string resources_name;
        public int unity_id;
        
        public Action<int, ResourcesType> SetNewValue;
        public ResourcesType ResourceType => (ResourcesType)unity_id;

        public int Value
        {
            get => resources_value;

            set
            {
                resources_value = value;
                SetNewValue?.Invoke(value, ResourceType);
            }
        }
    }
    
    public enum ResourcesType
    {
        GoldenBean = 2,
        Bean = 1,
        Corn = 3,
        Pease = 4,
        Seed = 5,
        GoldenBeans = 6
    }

    [Serializable]
    public class PlayerBalancesData
    {
        public bool success;
        public PlayerResourcesData[] balances;
    }

}