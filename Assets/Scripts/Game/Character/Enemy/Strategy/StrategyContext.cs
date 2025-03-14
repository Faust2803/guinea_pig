using UnityEngine;

namespace Strategy
{
    public class StrategyContext
    {
        private IStrategy _strategy;

        public void SetStrategy(IStrategy strategy)
        {
            _strategy = strategy;
        }

        public void Execute()
        {
            if (_strategy == null)
            {
                Debug.Log("error no current strategy");
                return;
            }

            _strategy.Execute();
        }
        
    }
}