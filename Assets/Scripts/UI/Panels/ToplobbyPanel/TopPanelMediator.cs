using System.Collections.Generic;
using DataModels.PlayerData;
using Game.Jumper;
using Managers;
using UnityEngine;
using Zenject;

namespace UI.Panels.ToplobbyPanel
{
    public class TopPanelMediator : BasePanelMediator <TopPanelView, PanelData>
    {
        [Inject] private LeaderboardManager _leaderboardManager;
        private PlayerManager _playerManager = ProjectContext.Instance.Container.Resolve<PlayerManager>();
        private Dictionary<ResourcesType, CurrencyItem> _currencyItems = new Dictionary<ResourcesType, CurrencyItem>();
        
        protected override void ShowStart()
        {
            base.ShowStart();
            // foreach (var t in _playerManager.Resources)
            // {
            //     t.Value.SetNewValue += UpdateResource;
            //     var currencyItem = Target.InstantiateCurrencyItem();
            //     currencyItem.SetCurrencyData(t.Value.Value, t.Value.ResourceType);
            //     _currencyItem.Add(t.Value.ResourceType, currencyItem);
            // }
            
            _playerManager.Resources[ResourcesType.GoldenBean].SetNewValue += UpdateResource;
            var currencyItem = Target.InstantiateCurrencyItem();
            currencyItem.SetCurrencyData(
                _playerManager.Resources[ResourcesType.GoldenBean].Value,
                _playerManager.Resources[ResourcesType.GoldenBean].ResourceType);
            _currencyItems.Add(_playerManager.Resources[ResourcesType.GoldenBean].ResourceType, currencyItem);

            Target.MyRecord.text = _leaderboardManager.MyLeaderboardData != null ? string.Format("{0} km", NumberFormatter.FormatValue(ConvertToGameMetersFromPos(_leaderboardManager.MyLeaderboardData.maxscore))) : "0";
        }

        private LayerSettings[] Map => Target.Map;

        private void UpdateResource(int value, ResourcesType type)
        {
            _currencyItems[type].UpdateValue(value);
        }

        private int ConvertToGameMetersFromPos(float y)
        {
            var sphereId = GetSphereByPosY(y);
            var percent = GetPercentOfSphereByObjectPos(y);
            return ConvertToGameMeters(sphereId, percent);
        }

        private int ConvertToGameMeters(int layerId, float percent)
        {
            var startHeight = GetStartLayerHeight(layerId);
            var heightInLayer = Map[layerId].Height * percent;
            var resultPercent = (startHeight + heightInLayer) / totalHeight;
            var result = (int)(resultPercent * GameLayerController.TARGET_METERS_AMOUNT);
            return result;
        }

        private float totalHeight
        {
            get
            {
                var result = 0f;
                for (var x = 0; x < Map.Length; x++)
                    result += Map[x].Height;
                return result;
            }
        }

        private int GetSphereByPosY(float y)
        {
            var max = 0f;
            for (var x = 0; x < Map.Length; x++)
            {
                max += Map[x].Height;
                if (y < max)
                    return x;
            }
            if (y > max) return Map.Length - 1;
            return -1;
        }

        private float GetPercentOfSphereByObjectPos(float objectPosY)
        {
            var sphere = Mathf.Clamp(GetSphereByPosY(objectPosY), 0, Map.Length - 1);
            var min = GetStartLayerHeight(sphere);
            var max = Map[sphere].Height;
            return (objectPosY - min) / max;
        }

        private int GetStartLayerHeight(int layerIdx)
        {
            var result = 0f;
            for (var x = 0; x < layerIdx && x < Map.Length; x++)
            {
                result += Map[x].Height;
            }
            return (int)result;
        }
    }
}