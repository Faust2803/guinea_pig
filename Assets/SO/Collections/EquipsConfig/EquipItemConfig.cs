using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "EquipItemConfig", menuName = "SO Equip Item Config", order = 0)]
    public class EquipItemConfig : ScriptableObject
    {
        public  int unity_id;
        public string spriteID;
        public EquipSlotType type;
        public Texture2D sprite;
        public GameObject gameObject_model;
    }

    public enum EquipSlotType
    {
        Skin = 0,
        Cloth = 1,
        Hat = 2
    }
}