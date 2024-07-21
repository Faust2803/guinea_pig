using System;
using Newtonsoft.Json;

namespace DataModels.MoonBank
{
    [Serializable]
    public class MoonInfoData
    {
        [JsonProperty("moon_bank")]
        public int BankAmount { get; set; }
        
        [JsonProperty("user_name")]
        public string UserName { get; set; }
        
        [JsonProperty("time_left")]
        public TimeLeftData TimeLeftInfo { get; set; }

        [JsonProperty("equipment")]
        public EquipmentHamsterMoon Equipment { get; set; }
    }

    [Serializable]
    public class TimeLeftData
    {
        public string hours { get; set; }
        public string minutes { get; set; }
        public string seconds { get; set; }
    }

    [Serializable]
    public class EquipmentHamsterMoon
    {
        [JsonProperty("collection_id")]
        public string CollectionId { get; set; }
        
        [JsonProperty("collection_sprite_name")]
        public string SpriteName { get; set; }
        
        [JsonProperty("items")]
        public EquipmentHamsterMoonItem[] Items { get; set; }
    }

    [Serializable]
    public class EquipmentHamsterMoonItem
    {
        [JsonProperty("unity_id")]
        public int Id { get; set; }
        
        [JsonProperty("item_name")]
        public string Name { get; set; }
    }
}