using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "CharacterConfig", menuName = "SO Character Config", order = 0)]
    public class CharacterConfig : ScriptableObject
    {
        public  GameObject [] CharacterPrefab = null;
    }
}