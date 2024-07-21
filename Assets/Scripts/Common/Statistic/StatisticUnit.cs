using System;

namespace Common.Statistic
{
    public interface IStatisticUnitInfo
    {
        event Action<int, IStatisticUnitInfo> OnIncreased;
        StatisticSourceId Id { get; }
        int Amount { get; }
        int IncreasedAmount { get; }
        int TotalAmount { get; }
    }

    public interface IStatisticUnit : IStatisticUnitInfo
    {
        void Increase(int value);
        void ApplyIncreased();
        void ResetIncreased();
    }
    
    public class StatisticUnit : IStatisticUnit
    {
        public event Action<int, IStatisticUnitInfo> OnIncreased;
        public StatisticSourceId Id { get; }
        public int Amount => _amount;
        public int IncreasedAmount => _increased;
        public int TotalAmount => _amount + _increased;
        private int _amount;
        private int _increased;

        public StatisticUnit(StatisticSourceId id, int amount)
        {
            Id = id;
            _amount = amount;
        }

        public void Increase(int value)
        {
            if (value < 1) return;
            
            _increased += value;
            OnIncreased?.Invoke(value, this);
        }

        public void ApplyIncreased()
        {
            _amount += _increased;
            _increased = 0;
        }

        public void ResetIncreased()
        {
            _increased = 0;
        }
    }
}