using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.ProfileWindow
{
    public class StatisticItemView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private TMP_Text _countText;
        [SerializeField] private Image _icon;

        public void Init(string label, int count)
        {
            _nameText.text = label;
            _countText.text = count.ToString();
            _icon.gameObject.SetActive(false);
        }
        
        public void Init(string label, int count, Sprite icon)
        {
            _nameText.text = label;
            _countText.text = count.ToString();
            _icon.gameObject.SetActive(true);
            _icon.sprite = icon;
        }
    }
}