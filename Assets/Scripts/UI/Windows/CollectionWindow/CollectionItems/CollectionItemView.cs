using DataModels.CollectionsData;
using TMPro;
using UI.Panels.ToplobbyPanel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.CollectionWindow.CollectionItems
{
    public class CollectionItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _hamsterName;
        public TextMeshProUGUI HamsterName => _hamsterName;

        [Header("Unlocked State")]
        [SerializeField] private GameObject _unlockedState;
        [SerializeField] private GameObject _defaultState;
        [SerializeField] private GameObject _selectedState;
        [SerializeField] private Image _unlockedHamsterIcon;
        [SerializeField] private Button _unlockedButton;
        [SerializeField] private GameObject[] _hpIndicators;
        [SerializeField] private GameObject[] _rarityIndicators;
        [SerializeField] private GameObject _priceIndicator;
        [SerializeField] private GameObject _equippedIndicator;
        [SerializeField] private CurrencyItem _currencyItem;

        public GameObject UnlockedState => _unlockedState;
        public GameObject DefaultState => _defaultState;
        public GameObject SelectedState => _selectedState;
        public Image UnlockedHamsterIcon => _unlockedHamsterIcon;
        public Button UnlockedButton => _unlockedButton;
        public GameObject[] HPIndicators => _hpIndicators;
        public GameObject[] RarityIndicators => _rarityIndicators;

        [Header("Locked State")]
        [SerializeField] private GameObject _lockedState;
        [SerializeField] private Image _lockedHamsterIcon;
        [SerializeField] private GameObject _selectedLockedState;

        public GameObject LockedState => _lockedState;
        public Image LockedHamsterIcon => _lockedHamsterIcon;

        private CollectionItemDataModel _currentCollectionItemData = null;
        public CollectionItemDataModel CollectionItemDataModel => _currentCollectionItemData;

        public void UpdateItemView(CollectionItemDataModel collectionItemDataModel)
        {
            _currentCollectionItemData = collectionItemDataModel;

            //Debug.Log($"Updated {collectionItemDataModel.collection_name}");
            _hamsterName.text = collectionItemDataModel.collection_name;

            _unlockedState.SetActive(collectionItemDataModel.collection_status == 1 || collectionItemDataModel.collection_is_shop == 1);
            _defaultState.SetActive(collectionItemDataModel.collection_status == 1 || collectionItemDataModel.collection_is_shop == 1);
            //_selectedState.SetActive(collectionItemDataModel.collection_equiped == 1);

            //_unlockedButton.interactable = collectionItemDataModel.collection_equiped == 0;
            _priceIndicator.SetActive(collectionItemDataModel.collection_status == 0 && collectionItemDataModel.collection_is_shop == 1);
            _currencyItem.SetCurrencyData(collectionItemDataModel.collection_cost, (DataModels.PlayerData.ResourcesType)collectionItemDataModel.collection_cost_type);
            _equippedIndicator.SetActive(collectionItemDataModel.collection_status == 1 && collectionItemDataModel.collection_equiped == 1);

            _unlockedHamsterIcon.sprite = collectionItemDataModel.spriteIcon;
            _hpIndicators[collectionItemDataModel.collection_hp - 2].SetActive(true);
            _rarityIndicators[collectionItemDataModel.collection_rarity].SetActive(true);

            _lockedState.SetActive(collectionItemDataModel.collection_status == 0 && collectionItemDataModel.collection_is_shop != 1);
            _lockedHamsterIcon.sprite = collectionItemDataModel.spriteIcon;
        }

        public void SetSelectedState()
        {
            if (_lockedState.activeSelf)
            {
                _selectedLockedState.SetActive(true);
            }
            else
            {
                _defaultState.SetActive(false);
                _priceIndicator.SetActive(false);
                _equippedIndicator.SetActive(false);

                _selectedState.gameObject.SetActive(true);
            }
        }

        public void SetDefaultState()
        {
            if (_lockedState.activeSelf)
            {
                _selectedLockedState.SetActive(false);
            }
            else
            {
                _defaultState.SetActive(true);
                _priceIndicator.SetActive(_currentCollectionItemData.collection_status == 0);
                _equippedIndicator.SetActive(_currentCollectionItemData.collection_equiped == 1);

                _selectedState.gameObject.SetActive(false);
            }
        }
    }
}


