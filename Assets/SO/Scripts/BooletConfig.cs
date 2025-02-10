using UnityEngine;

namespace SO.Scripts
{
    [CreateAssetMenu(fileName = "BooletConfig", menuName = "SO Boolet Config", order = 0)]
    public class BooletConfig : ScriptableObject
    {
        public  GameObject [] BooletPrefab = null;
    }
}