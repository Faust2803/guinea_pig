
using DataModels.CollectionsData;
using System.Collections.Generic;
using UI.Panels.ToplobbyPanel;
using UI.Windows.CollectionWindow.CollectionItems;
using UnityEngine;

namespace UI.Windows.MarketWindow
{
    public class MarketWindowView : BaseWindowView
    {
        [SerializeField] private Transform _collectionItemTransform;
        [SerializeField] private GameObject _marketItem;
        [SerializeField] private CurrencyItem _currencyItem;

        private List<MarketItemView> _generatedCollectionItems = new List<MarketItemView>();

        public Transform CollectionItemTransform => _collectionItemTransform;
        public CurrencyItem CurrencyItem => _currencyItem;

        protected override void CreateMediator()
        {
            _mediator = new MarketWindowMediator();
        }

        internal void UpdateWindowView(Dictionary<int, CollectionItemDataModel> playerCollection)
        {
            int index = 0;
            foreach (var collectionItem in playerCollection)
            {
                _generatedCollectionItems[index].UpdateItemView(collectionItem.Value);
                index++;
            }
        }

        public MarketItemView InstantiateMarketItem()
        {
            var marketItemView = Instantiate(_marketItem, _collectionItemTransform).GetComponent<MarketItemView>();
            _generatedCollectionItems.Add(marketItemView);
            return marketItemView;
        }
    }
}