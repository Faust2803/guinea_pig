using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "EnvironmentConfig", menuName = "SO Environment Config", order = 0)]
    public class EnvironmentConfig : ScriptableObject
    {
        public  GameObject [] EnvironmentPrefab = null;
    }
}