using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "CollectionsConfig", menuName = "SO Collections Config", order = 0)]
    public class CollectionsConfig : ScriptableObject
    {
        public  ItemConfig [] CollectionsItems = null;
    }
}