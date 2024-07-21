using System.Collections.Generic;

namespace Common.Statistic
{
    public class StatisticStorage
    {
        private StatisticUnit[] _units;

        public StatisticStorage(ICollection<StatisticUnit> units)
        {
            _units = new StatisticUnit[units.Count];
        }

        public StatisticUnit GetUnit(StatisticSourceId id)
        {
            return null;
        }
    }
}