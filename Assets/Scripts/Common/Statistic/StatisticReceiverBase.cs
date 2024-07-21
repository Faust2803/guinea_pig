using System;
using Common.Signals.StatisticSignal;
using Zenject;

namespace Common.Statistic
{
    public abstract class StatisticReceiverBase : IDisposable
    {
        public StatisticSourceId TargetId => _targetId;
        private SignalBus _signalBus;
        private StatisticSourceId _targetId;
        
        protected StatisticReceiverBase(StatisticSourceId target, SignalBus signalBus)
        {
            _signalBus = signalBus;
            _signalBus.Subscribe<StatisticSignal>(Received);
            _targetId = target;
        }
        
        private void Received(StatisticSignal signal)
        {
            if(signal.Id == _targetId)
                Received(signal.Amount);
        }

        protected abstract void Received(int value);

        public virtual void Dispose()
        {
            _signalBus.Unsubscribe<StatisticSignal>(Received);
        }
    }
}