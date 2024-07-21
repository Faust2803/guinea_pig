using DataModels.CollectionsData;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI.Views
{
    public class HamsterPreviewViewBase : MonoBehaviour
    {
        [SerializeField] private RectTransform _modelParent;
        
        [SerializeField] private HPView[] _hpIndicators;

        private HPView _currentHpPreview = null;

        public RectTransform ModelParent => _modelParent;

        public virtual void UpdatePreview(CollectionItemDataModel collectionItemDataModel)
        {
            InitialPreparation();

            if (collectionItemDataModel.collection_status == 1 || collectionItemDataModel.collection_is_shop == 1)
            {
                _currentHpPreview = _hpIndicators[collectionItemDataModel.collection_hp - 2];
                _currentHpPreview.gameObject.SetActive(true);
            }
        }

        public virtual void UpdatePreviewAndHPPreview(CollectionItemDataModel collectionItemDataModel, bool updateHpPreview = true)
        {
            UpdatePreview(collectionItemDataModel);
            UpdateHpCount(collectionItemDataModel.collection_current_hp, _currentHpPreview);
        }

        public virtual void InitialPreparation()
        { 
            if(_hpIndicators != null && _hpIndicators.Length > 0)
                for(int i = 0; i < _hpIndicators.Length; i++)
                    _hpIndicators[i].gameObject.SetActive(false);
        }

        private void UpdateHpCount(int currentHpCount, HPView hpPreview)
        {
            hpPreview.ResetHPSlots();
            hpPreview.UpdatePreview(currentHpCount);
        }
    }
}
