using DataModels.CollectionsData;
using SO.Scripts;
using UnityEngine;
using Zenject;

namespace Managers
{
    public class CollectionManager
    {
        [Inject] private CollectionsConfig _collectionsConfig;
        [Inject] private EquipsConfig _equipsConfig;

        public void SetUpCollection(CollectionItemDataModel[] collection)
        {
            for (var i = 0; i < collection.Length; i++)
            {
                collection[i].spriteIcon = GetCollectionItemSprite(collection[i].collection_sprite_name);
                SetupCollectionItem(collection[i]);
                // for (var j = 0; j < collection[i].items.Length; j++)
                // {
                //     var cfg = FindItemConfig(collection[i].items[j].unity_id);
                //     if(cfg == null)
                //     {
                //         Debug.Log($"<color=yellow>Config not finded by unity item id [{collection[i].items[j].unity_id}]</color>");
                //         continue;
                //     }
                //
                //     collection[i].items[j].sprite_name = cfg.sprite;
                //     collection[i].items[j].gameObject_model = cfg.gameObject_model;
                //     collection[i].items[j].slotType = cfg.type;
                //     //SetEquipItem(collection[i].items[j]);
                // }
            }
        }

        public void SetupCollection(PyramidCollectionItemDTO[] collection)
        {
            foreach (var itemDto in collection)
                SetupCollectionItem(itemDto);
        }
        
        public void SetupCollectionItem(IHamsterVisualData itemData)
        {
            for (var i = 0; i < itemData.Equipment.Length; i++)
            {
                var equipItem = itemData.Equipment[i];
                var cfg = FindItemConfig(equipItem.unity_id);
                if(cfg == null)
                {
                    Debug.Log($"<color=yellow>Config not finded by unity item id [{equipItem.unity_id}]</color>");
                    continue;
                }

                equipItem.sprite_name = cfg.sprite;
                equipItem.gameObject_model = cfg.gameObject_model;
                equipItem.slotType = cfg.type;
            }
        }

        private Sprite GetCollectionItemSprite(int unity_id)
        {
            for (var i = 0; i < _collectionsConfig.CollectionsItems.Length; i++)
            {
                if(_collectionsConfig.CollectionsItems[i].unity_id == unity_id)
                {
                    return _collectionsConfig.CollectionsItems[i].sprite;
                }
            }

            return null;
        }

        private Sprite GetCollectionItemSprite(string unity_id)
        {
            for (var i = 0; i < _collectionsConfig.CollectionsItems.Length; i++)
            {
                if (_collectionsConfig.CollectionsItems[i].spriteID == unity_id)
                {
                    return _collectionsConfig.CollectionsItems[i].sprite;
                }
            }

            return null;
        }

        private Texture2D GetEquipItemSprite(int unity_id)
        {
            for (var i = 0; i < _equipsConfig.EquipItems.Length; i++)
            {
                if(_equipsConfig.EquipItems[i].unity_id == unity_id)
                {
                    return _equipsConfig.EquipItems[i].sprite;
                }
            }
            return null;
        }

        private EquipItemConfig FindItemConfig (int itemUnityId)
        {
            for (var i = 0; i < _equipsConfig.EquipItems.Length; i++)
            {
                if (_equipsConfig.EquipItems[i].unity_id == itemUnityId)
                {
                    return _equipsConfig.EquipItems[i];
                }
            }

            return null;
        }

        private void SetEquipItem(EquipItem equipItem)
        {
            for (var i = 0; i < _equipsConfig.EquipItems.Length; i++)
            {
                if(_equipsConfig.EquipItems[i].unity_id == equipItem.unity_id)
                {
                    equipItem.sprite_name = _equipsConfig.EquipItems[i].sprite;
                    equipItem.gameObject_model = _equipsConfig.EquipItems[i].gameObject_model;
                    equipItem.slotType = _equipsConfig.EquipItems[i].type;
                    return;
                }
            }
        }
    }
}