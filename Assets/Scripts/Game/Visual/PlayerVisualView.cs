using DataModels.CollectionsData;
using Managers;
using SO.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Game.Jumper
{
    public class PlayerVisualView : MonoBehaviour
    {
        private const string MAT_SKIN_PARAM = "_BaseSkin";
        private const string MAT_CLOTH_PARAM = "_OverlayCloth";
        private const string MAT_HAS_CLOTH_PARAM = "_HasCloth";

        public bool AutosetupOnStart = true;

        [SerializeField] SkinnedMeshRenderer[] body;
        [SerializeField] Transform hatsContainer;
        [SerializeField, Tooltip("When collection dont use skin")] bool setupDefaultSkin = true;
        [SerializeField] Texture2D defaultSkin;

        [Inject] PlayerManager playerData;

        private Dictionary<int, GameObject> hatsTable = new Dictionary<int, GameObject>();
        private Material currentMaterial;

        private void Awake()
        {
            ProjectContext.Instance.Container.Inject(this);
            InitMaterials();
        }

        private void Start()
        {
            if(AutosetupOnStart)
            {
                SetupCollection(playerData.CurrentCollectionItem);
            }
        }

        public void SetupCollection (IHamsterVisualData collection)
        {
            InitMaterials();

            var hasSkin = hasItem(EquipSlotType.Skin);
            var hasCloth = hasItem(EquipSlotType.Cloth);
            var hasHat = hasItem(EquipSlotType.Hat);
            Debug.Log($"Setup collection: {JsonUtility.ToJson(collection, true)}");
            for (var x = 0; x < collection.Equipment.Length; x++)
                SetupEquipElement(collection.Equipment[x]);

            SetEnabledClothes(hasCloth);
            if (hasSkin == false && setupDefaultSkin)
                SetBodyTextures(defaultSkin, false);

            if (!hasHat)
                EnableHatInTable(-1);

            bool hasItem(EquipSlotType type)
            {
                foreach (var item in collection.Equipment)
                    if (item.slotType == type)
                        return true;
                return false;
            }
        }

        private void InitMaterials()
        {
            if (currentMaterial == null)
            {
                currentMaterial = new Material(body[0].material);
                for (var x = 0; x < body.Length; x++)
                {
                    body[x].SetMaterials(new List<Material>() { currentMaterial });
                }
            }
        }

        internal void SetupEquipElement (EquipItem item)
        {
            switch(item.slotType)
            {
                case EquipSlotType.Skin:
                    SetBodyTextures(item.sprite_name, false);
                    break;

                case EquipSlotType.Cloth:
                    SetBodyTextures(item.sprite_name, true);
                    break;

                case EquipSlotType.Hat:
                    if (hatsTable.ContainsKey(item.unity_id) == false)
                    {
                        var hat = Instantiate(item.gameObject_model, hatsContainer);

                        hat.transform.localPosition = item.gameObject_model.transform.localPosition;
                        hat.transform.localRotation = item.gameObject_model.transform.localRotation;

                        hatsTable.Add(item.unity_id, hat);
                    }

                    EnableHatInTable(item.unity_id);
                    break;
            }
        }

        private void EnableHatInTable (int key)
        {
            for(var x = 0; x < hatsTable.Count; x++)
            {
                var enabled = hatsTable.ElementAt(x).Key == key;
                hatsTable.ElementAt(x).Value?.SetActive(enabled);
            }
        }

        private void SetEnabledClothes (bool value)
        {
            currentMaterial.SetInt(MAT_HAS_CLOTH_PARAM, value ? 1 : 0);
        }

        private void SetBodyTextures(Texture2D texture, bool isCloth)
        {
            currentMaterial.SetTexture(isCloth ? MAT_CLOTH_PARAM : MAT_SKIN_PARAM, texture);
        }
    }
}