using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Managers;
using Newtonsoft.Json;

namespace Common.Statistic.DataDto
{
    [Serializable]
    public class StatisticDataDto 
    {
        public StatisticUnitDataDto[] UnitData { get; set; }
    }

    [Serializable]
    public class StatisticUnitDataDto
    {
        public StatisticSourceId Id { get; set; }
        public int Amount { get; set; }
    }

    [Serializable]
    public class StatisticSimpleData
    {
        [JsonProperty("total_beans_collected")]
        public int TotalBeans { get; set; }
        [JsonProperty("total_games")]
        public int TotalGames { get; set; }
    }

    public class StatisticNetworkFacade
    {
        private INetworkManager _networkManager;

        public StatisticNetworkFacade(INetworkManager manager)
        {
            _networkManager = manager;
        }
        
        // public async UniTask<StatisticDataDto> GetStatisticData()
        // {
        //     
        // }
        //
        // public async UniTaskVoid SaveStatisticData(StatisticUnitDataDto data)
        // {
        //     
        // }
    }

    public class StatisticManager
    {
        private StatisticUnit[] _units = Array.Empty<StatisticUnit>();
        private bool _handleRequest;
        private Queue<UniTask> _requestQueue = new Queue<UniTask>();

        private async UniTaskVoid RequestHandler()
        {
            while (_handleRequest)
            {
                if (_requestQueue.Count == 0)
                {
                    await UniTask.WaitForSeconds(1f);
                    continue;
                }

                var request = _requestQueue.Dequeue();
                await request;
                await UniTask.WaitForSeconds(0.5f);
            }
        }
    }
}