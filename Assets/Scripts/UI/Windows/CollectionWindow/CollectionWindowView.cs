
using DataModels.CollectionsData;
using SO.Scripts;
using System;
using System.Collections.Generic;
using UI.Panels.ToplobbyPanel;
using UI.Views;
using UI.Windows.CollectionWindow.CollectionItems;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Windows.CollectionWindow
{
    public class CollectionWindowView : BaseWindowView
    {
        [SerializeField] private Transform  _collectionItemTransform;
        [SerializeField] private GameObject  _collectionItem;
        [SerializeField] private HamsterPreviewView _hamsterPreviewView;
        [SerializeField] private RarityCardsData _rarityCardsData;

        [SerializeField] private Button _buyButton;
        [SerializeField] private Button _equipButton;
        [SerializeField] private GameObject _equippedButton;
        [SerializeField] private CurrencyItem _currencyItem;
        [SerializeField] private CurrencyItem _currencyPreviewItem;
        

        private bool _onTop = false;
        private bool _onBottom = false;

        public Button BuyButton => _buyButton;
        public Button EquipButton => _equipButton;

        public CurrencyItem CurrencyItem => _currencyItem;

        private List<CollectionItemView> _generatedCollectionItems = new List<CollectionItemView>();

        public HamsterPreviewView HamsterPreviewView => _hamsterPreviewView;

        internal void UpdateWindowView(Dictionary<int, CollectionItemDataModel> playerCollection)
        {
            int index = 0;
            foreach (var collectionItem in playerCollection)
            {
                _generatedCollectionItems[index].UpdateItemView(collectionItem.Value);
                index++;

                if (collectionItem.Value.collection_equiped == 1)
                    _hamsterPreviewView.UpdatePreview(collectionItem.Value);
            }
        }

        internal void UpdateWindowView(CollectionItemDataModel collectionItemData)
        {
            _hamsterPreviewView.UpdatePreview(collectionItemData);

            _buyButton.gameObject.SetActive(collectionItemData.collection_status == 0 && collectionItemData.collection_is_shop == 1);
            _currencyPreviewItem.gameObject.SetActive(_buyButton.gameObject.activeSelf);
            _currencyPreviewItem.SetCurrencyData(collectionItemData.collection_cost, DataModels.PlayerData.ResourcesType.GoldenBean);

            _equipButton.gameObject.SetActive(collectionItemData.collection_status == 1 && collectionItemData.collection_equiped == 0);
            _equippedButton.SetActive(collectionItemData.collection_status == 1 && collectionItemData.collection_equiped == 1);
        }

        protected override void CreateMediator()
        {
            _mediator = new CollectionWindowMediator();
        }

        public Transform CollectionItemTransform => _collectionItemTransform;

        public CollectionItemView InstantiateCollectioItem()
        {
            var collectionItemView = Instantiate(_collectionItem, _collectionItemTransform).GetComponent<CollectionItemView>();
            _generatedCollectionItems.Add(collectionItemView);
            return collectionItemView;
        }
    }
}