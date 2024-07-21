using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "WindowsConfig", menuName = "SO Windows Config", order = 0)]
    public class WindowsConfig : ScriptableObject
    {
        public  GameObject [] WindowsPrefab = null;
    }
}