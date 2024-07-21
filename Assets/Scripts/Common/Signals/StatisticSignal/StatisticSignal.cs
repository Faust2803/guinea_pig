using Common.Statistic;

namespace Common.Signals.StatisticSignal
{
    public class StatisticSignal
    {
        public readonly StatisticSourceId Id; 
        public readonly int Amount;

        public StatisticSignal(StatisticSourceId id, int amount)
        {
            Id = id;
            Amount = amount;
        }
    }
}