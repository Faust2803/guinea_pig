using UnityEngine;

namespace Strategy
{
    public class Patrol : IStrategy
    {
        public void Execute()
        {
            Debug.Log("Patrol");
        }
    }
}