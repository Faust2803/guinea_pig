
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.WalletWindow
{
    public class WalletWindowView : BaseWindowView
    {
        [SerializeField] private Button  _addMoney;
        [SerializeField] private Button  _buyItem;
        [SerializeField] private Button  _equipItem;
        [SerializeField] private Button _exitButton;
        [SerializeField] private TMP_InputField   _equipItemField;
        [SerializeField] private TMP_InputField   _buyItemField;
        
        [SerializeField] private Button  _prachute;
        [SerializeField] private Button  _reactiveFart;
        [SerializeField] private Button  _achiveTermosphere;
        [SerializeField] private Button _achiveSpace;
        
        protected override void CreateMediator()
        {
            _mediator = new WalletWindowMediator();
        }

        public Button AddMoney => _addMoney;
        public Button BuyItem => _buyItem;
        public Button EquipItem => _equipItem;
        public Button Prachute => _prachute;
        public Button ReactiveFart => _reactiveFart;
        public Button ExitButton => _exitButton;
        public Button AchiveTermosphere => _achiveTermosphere;
        public Button AchiveSpace => _achiveSpace;


        public string EquipItemField
        {
            get => _equipItemField.text;
            set => _equipItemField.text = value;
        }
        
        public string BuyItemField
        {
            get => _buyItemField.text;
            set => _buyItemField.text = value;
        }
    }
}