using System;
using UnityEngine;

namespace DataModels.CollectionsData
{
    [Serializable]
    public class MarcetDataModel
    {
        public CollectionItemDataModel[] market;
    }
    
    [Serializable]
    public class CollectionDataModel
    {
        public bool success;
        public CollectionItemDataModel[] player_data;
    }

    [Serializable]
    public class CollectionItemDataModel : IHamsterVisualData
    {
        public int collection_id;
        public int collection_cost;
        public int collection_cost_type;
        public int collection_hp;
        public int collection_current_hp;
        public int collection_status;
        public int collection_equiped;
        public string collection_name;
        public string collection_description;
        public int collection_rarity;
        public string collection_sprite_name;
        public int collection_is_shop;
        public Sprite spriteIcon;
        public EquipItem[] items;
        
        public Action<int, int> SetNewHp;
        
        public int HP
        {
            get => collection_current_hp;
            set
            {
                collection_current_hp = value;
                SetNewHp?.Invoke(collection_id, collection_current_hp);
            }
        }
        public int Id => collection_id;
        public string SpriteName => collection_sprite_name;
        public EquipItem[] Equipment => items;
    }
}