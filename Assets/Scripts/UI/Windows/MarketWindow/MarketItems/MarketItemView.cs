using DataModels.CollectionsData;
using SO.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Panels.ToplobbyPanel;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Windows.MarketWindow
{
    public class MarketItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _hamsterName;
        public TextMeshProUGUI HamsterName => _hamsterName;

        [Header("Unlocked State")]
        [SerializeField] private GameObject _unlockedState;
        [SerializeField] private Image _unlockedHamsterIcon;
        [SerializeField] private Button _buyButton;
        [SerializeField] private CurrencyItem _currencyItem;
        [SerializeField] private GameObject[] _hpIndicators;
        [SerializeField] private GameObject[] _rarityIndicators;

        public GameObject UnlockedState => _unlockedState;
        public Image UnlockedHamsterIcon => _unlockedHamsterIcon;
        public Button BuyButton => _buyButton;
        public CurrencyItem CurrencyItem => _currencyItem;
        public GameObject[] HPIndicators => _hpIndicators;
        public GameObject[] RarityIndicators => _rarityIndicators;


        [Header("Locked State")]
        [SerializeField] private GameObject _lockedState;
        [SerializeField] private Image _lockedHamsterIcon;

        public GameObject LockedState => _lockedState;
        public Image LockedHamsterIcon => _lockedHamsterIcon;

        public void UpdateItemView(CollectionItemDataModel _collectionItemDataModel)
        {
            Debug.Log($"Updated {_collectionItemDataModel.collection_name}");
            _hamsterName.text = _collectionItemDataModel.collection_name;

            _unlockedState.gameObject.SetActive(_collectionItemDataModel.collection_status == 0);
            _buyButton.interactable = _collectionItemDataModel.collection_status == 0;
            _currencyItem.SetCurrencyData(_collectionItemDataModel.collection_cost, (DataModels.PlayerData.ResourcesType)_collectionItemDataModel.collection_cost_type);

            _unlockedHamsterIcon.sprite = _collectionItemDataModel.spriteIcon;
            _hpIndicators[_collectionItemDataModel.collection_hp - 2].SetActive(true);
            _rarityIndicators[_collectionItemDataModel.collection_rarity].SetActive(true);

            _lockedState.gameObject.SetActive(_collectionItemDataModel.collection_status == 1);
            _lockedHamsterIcon.sprite = _collectionItemDataModel.spriteIcon;
        }
    }
}