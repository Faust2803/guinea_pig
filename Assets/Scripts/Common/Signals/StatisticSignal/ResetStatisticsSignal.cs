using System.Collections.Generic;
using Common.Statistic;

namespace Common.Signals.StatisticSignal
{
    public class ResetStatisticsSignal
    {
        public readonly ICollection<StatisticSourceId> Ids;
        
        public ResetStatisticsSignal(ICollection<StatisticSourceId> idsForReset)
        {
            Ids = idsForReset;
        }
    }
}