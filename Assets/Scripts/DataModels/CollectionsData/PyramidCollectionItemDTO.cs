using System;
using Newtonsoft.Json;

namespace DataModels.CollectionsData
{
    [Serializable]
    public class PyramidCollectionItemDTO : IHamsterVisualData
    {
        [JsonProperty("collection_id")]
        public int Id { get; set; }
        
        [JsonProperty("collection_equiped")]
        public int Equiped { get; set; }
        
        [JsonProperty("collection_sprite_name")]
        public string SpriteName { get; set; }
        
        [JsonProperty("items")]
        public EquipItem[] Equipment { get; set; }
    }
}