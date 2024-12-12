using System.Collections.Generic;
using DataModels.ResourcesData;
using Managers;
using Zenject;

namespace UI.Panels.ToplobbyPanel
{
    public class TopPanelMediator : BasePanelMediator <TopPanelView, PanelData>
    {

        private PlayerManager _playerManager = ProjectContext.Instance.Container.Resolve<PlayerManager>();
        private Dictionary<ResourcesType, CurrencyItem> _currencyItems = new Dictionary<ResourcesType, CurrencyItem>();
        
        protected override void ShowStart()
        {
            base.ShowStart();
        }

        private void UpdateResource(int value, ResourcesType type)
        {
            _currencyItems[type].UpdateValue(value);
        }
    }
}