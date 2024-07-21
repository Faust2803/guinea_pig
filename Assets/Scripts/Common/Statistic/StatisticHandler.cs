using Zenject;

namespace Common.Statistic
{
    public class StatisticHandler : StatisticReceiverBase
    {
        private IStatisticUnit _unit;
        
        public StatisticHandler(IStatisticUnit unit, SignalBus signalBus) : base(unit.Id, signalBus)
        {
            _unit = unit;
        }

        protected override void Received(int value)
        {
            _unit.Increase(value);
        }
    }
}