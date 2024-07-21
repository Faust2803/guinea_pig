using DataModels.CollectionsData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class HamsterPreviewView : HamsterPreviewViewBase
    {
        [SerializeField] private TextMeshProUGUI _hamsterName;
        [SerializeField] private TextMeshProUGUI _hamsterDescription;
       
        [SerializeField] private GameObject[] _rarityIndicators;

        public TextMeshProUGUI HamsterName => _hamsterName;
        public TextMeshProUGUI HamsterDescription => _hamsterDescription;

        public override void UpdatePreview(CollectionItemDataModel collectionItemDataModel)
        {
            base.UpdatePreview(collectionItemDataModel);
            
            _hamsterName.text = collectionItemDataModel.collection_name;
            _hamsterDescription.text = collectionItemDataModel.collection_description;

            _rarityIndicators[collectionItemDataModel.collection_rarity].SetActive(true);
        }

        public override void InitialPreparation()
        {
            base.InitialPreparation();

            if (_rarityIndicators != null && _rarityIndicators.Length > 0)
                for (int i = 0; i < _rarityIndicators.Length; i++)
                    _rarityIndicators[i].SetActive(false);
        }
    }
}
