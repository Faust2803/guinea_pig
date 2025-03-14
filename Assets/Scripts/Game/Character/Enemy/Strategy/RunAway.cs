using UnityEngine;

namespace Strategy
{
    public class RunAway : IStrategy
    {
        public void Execute()
        {
            Debug.Log("RunAway");
        }
    }
}