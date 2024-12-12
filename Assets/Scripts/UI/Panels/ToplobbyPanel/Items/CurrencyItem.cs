using DataModels.ResourcesData;
using SO.Scripts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


namespace UI.Panels.ToplobbyPanel
{
    public class CurrencyItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI  _currencyValue;
        [SerializeField] private Image  _currencyImage;
        [SerializeField] private PlayerResourcesIconConfig _iconConfig;
        public void SetCurrencyData(int value, ResourcesType type)
        {
            SetIcon(_iconConfig.GetIconSprite(type));
            UpdateValue(value);
        }

        public void UpdateValue(int value)
        {
            _currencyValue.text = NumberFormatter.FormatValue(value);
        }

        public void SetIcon(Sprite sprite)
        {
            _currencyImage.sprite = sprite;
        }
    }
}


