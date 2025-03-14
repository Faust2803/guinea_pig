using UnityEngine;

namespace Strategy
{
    public class Fight : IStrategy
    {
        public void Execute()
        {
            Debug.Log("Fight");
        }
    }
}