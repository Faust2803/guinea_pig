
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.LeaderboardWindow
{
    public class LeaderboardWindowView : BaseWindowView
    {
        [SerializeField] private Transform _itemsParent;
        [SerializeField] private GameObject _leaderboardItemPrefab;
        
        protected override void CreateMediator()
        {
            _mediator = new LeaderboardWindowMediator();
        }

        public LeaderboardItemView InstantiateMarketItem()
        {
            var marketItemView = Instantiate(_leaderboardItemPrefab, _itemsParent).GetComponent<LeaderboardItemView>();
            return marketItemView;
        }
    }
}