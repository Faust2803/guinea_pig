using DataModels.CollectionsData;
using System.Collections.Generic;
using UnityEngine;

namespace UI.Windows.AchievementWindow
{
    public class AchievementWindowView : BaseWindowView
    {
        [SerializeField] private Transform _achievementItemTransform;
        [SerializeField] private GameObject _achievementItem;

        private List<AchievementItemView> _generatedAchievementItems = new List<AchievementItemView>();

        public Transform AchievementItemTransform => _achievementItemTransform;
        

        protected override void CreateMediator()
        {
            _mediator = new AchievementWindowMediator();
        }

        public AchievementItemView InstantiateMarketItem()
        {
            var marketItemView = Instantiate(_achievementItem, _achievementItemTransform).GetComponent<AchievementItemView>();
            _generatedAchievementItems.Add(marketItemView);
            return marketItemView;
        }
    }
}