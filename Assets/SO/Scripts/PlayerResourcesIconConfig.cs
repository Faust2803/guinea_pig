using DataModels.PlayerData;
using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "PlayerResourcesIconConfig", menuName = "SO Player Resources Icon Config", order = 0)]
    public class PlayerResourcesIconConfig : ScriptableObject
    {
        public  Sprite [] ResourceIcon = null;

        public Sprite GetIconSprite(ResourcesType type)
        {
            return ResourceIcon[(int)type];
        }
    }
}