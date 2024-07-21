using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "RarityCardsData", menuName = "SO Rarity Cards Data", order = 0)]
    public class RarityCardsData : ScriptableObject
    {
        public GameObject[] cardPrefabs = null;

        public GameObject this[int index] => cardPrefabs[index];
    }
}
