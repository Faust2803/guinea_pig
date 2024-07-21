using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "ItemConfig", menuName = "SO Item Config", order = 0)]
    public class ItemConfig : ScriptableObject
    {
        public  int unity_id;
        public string spriteID;
        public Sprite sprite;
    }
}