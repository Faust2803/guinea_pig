using TMPro;
using UnityEngine;

namespace UI.Panels.ToplobbyPanel
{
    public class TopPanelView : BasePanelView
    {

        [SerializeField] private Transform  _currencyItemTransform;
        [SerializeField] private GameObject  _currencyItem;
        [SerializeField] private TextMeshProUGUI  _myRecord;

        protected override void CreateMediator()
        {
            _mediator = new TopPanelMediator();
        }

        public CurrencyItem InstantiateCurrencyItem()
        {
             return  Instantiate(_currencyItem, _currencyItemTransform).GetComponent<CurrencyItem>();
        }
        
        public TextMeshProUGUI MyRecord => _myRecord;
    }
}