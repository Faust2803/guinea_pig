using SO.Scripts;
using System;
using UnityEngine;

namespace DataModels.CollectionsData
{
    [Serializable]
    public class EquipItem
    {
        public int unity_id;
        public string item_name;
        public EquipSlotType slotType;
        public Texture2D sprite_name;
        public GameObject gameObject_model;
    }
}