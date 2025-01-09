using UnityEngine;

namespace SO.Scripts.ThreeInRow
{
    [CreateAssetMenu(fileName = "CellConfig", menuName = "Configs/ThreeInRow/SO Cell Config", order = 0)]
    public class CellConfig : ScriptableObject
    {
        public  GameObject [] CellPrefab = null;
    }
}