using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "PanelsConfig", menuName = "SO Panels Config", order = 0)]
    public class PanelsConfig : ScriptableObject
    {
        public  GameObject [] PanelsPrefab = null;
    }
}