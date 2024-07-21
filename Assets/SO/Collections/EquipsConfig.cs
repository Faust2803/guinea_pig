using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "EquipsConfig", menuName = "SO Equips Config", order = 0)]
    public class EquipsConfig : ScriptableObject
    {
        public  EquipItemConfig [] EquipItems = null;
    }
}