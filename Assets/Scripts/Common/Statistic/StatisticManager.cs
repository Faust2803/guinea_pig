using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Signals.StatisticSignal;
using Cysharp.Threading.Tasks;
using Managers;
using UnityEngine;
using Zenject;

namespace Common.Statistic
{
    public interface IStatisticManager
    {
        IReadOnlyCollection<IStatisticUnitInfo> GetStatisticInfo();
        UniTask Initialize();
    }
    
    public class StatisticManager : IStatisticManager, IDisposable
    {
        private Dictionary<StatisticSourceId, IStatisticUnit> _unitMap = new Dictionary<StatisticSourceId, IStatisticUnit>();
        private IDisposable[] _handlers = Array.Empty<IDisposable>();
        private INetworkManager _networkManager;
        private SignalBus _signalBus;

        private bool _isInitialized = false;
        
        public StatisticManager(SignalBus signalBus, INetworkManager networkManager)
        {
            _networkManager = networkManager;
            _signalBus = signalBus;
            
            //Initialize();
        }

        public async UniTask Initialize()
        {
            if (_isInitialized) return;

           await CreateStatisticUnits();
           CreateStatisticHandlers();
           _signalBus.Subscribe<SaveStatisticSignal>(Save);
           _signalBus.Subscribe<ResetStatisticsSignal>(Reset);

            _isInitialized = true;
        }
        
        private async UniTask CreateStatisticUnits()
        {
            Debug.Log("Getting Statistics");
            var statistic = await _networkManager.GetStatistic(); //new PlayerStatistic();//_playerManager.PlayerStatistic;
            
            var unitBeam = new StatisticUnit(StatisticSourceId.Beans, statistic.TotalBeans);
            _unitMap.Add(StatisticSourceId.Beans, unitBeam);

            var unitGames = new StatisticUnit(StatisticSourceId.PlayGame, statistic.TotalGames);
            _unitMap.Add(StatisticSourceId.PlayGame, unitGames);
        }

        private void CreateStatisticHandlers()
        {
            _handlers = new IDisposable[_unitMap.Count];

            var i = 0;
            foreach (var unit in _unitMap.Values)
                _handlers[i++] = new StatisticHandler(unit, _signalBus);
        }

        public void Dispose()
        {
            _signalBus?.Unsubscribe<SaveStatisticSignal>(Save);
            _signalBus?.Unsubscribe<ResetStatisticsSignal>(Reset);
            _unitMap.Clear();
            foreach (var handler in _handlers) 
                handler.Dispose();
        }

        //TODO: Send info to the server
        private void Save(SaveStatisticSignal saveSignal)
        {
            Debug.Log($"[{GetType().Name}] Save statistic increased values.");

            foreach (var unit in _unitMap.Values)
                unit.ApplyIncreased();
        }

        private void Reset(ResetStatisticsSignal resetSignal)
        {
            if (resetSignal.Ids.Contains(StatisticSourceId.All))
            {
                foreach (var unit in _unitMap.Values)
                    unit.ResetIncreased();
                
                return;
            }

            foreach (var idToReset in resetSignal.Ids)
            {
                if(_unitMap.TryGetValue(idToReset, out var unit))
                    unit.ResetIncreased();
            }
        }

        public IReadOnlyCollection<IStatisticUnitInfo> GetStatisticInfo()
        {
            var result = new IStatisticUnitInfo[_unitMap.Count];

            var i = 0;
            
            foreach (var unit in _unitMap.Values)
                result[i++] = unit;

            return result;
        }
    }
}