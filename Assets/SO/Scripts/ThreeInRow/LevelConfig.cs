using UnityEngine;

namespace SO.Scripts.ThreeInRow
{
    [CreateAssetMenu(fileName = "LevelConfig", menuName = "Configs/ThreeInRow/SO Level Config", order = 0)]
    public class LevelConfig : ScriptableObject
    {
        public  GameObject [] LevelPrefab = null;
    }
}