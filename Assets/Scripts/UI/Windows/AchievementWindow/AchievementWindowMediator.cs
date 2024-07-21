using DataModels.PlayerData;
using Managers;
using System;
using System.Collections.Generic;
using DataModels.Achievement;
using Managers.Analytics;
using UI.Windows.SimpleDialogWindow;
using UnityEngine;
using Zenject;
using UI.Windows.RewardWindow;
using SO.Scripts;

namespace UI.Windows.AchievementWindow
{
    public class AchievementWindowMediator :BaseWindowMediator<AchievementWindowView, WindowData>
    {
        [Inject] private AchievementManager _achievementManager;
        [Inject] private PlayerManager _playerManager;
        [Inject] private PlayerResourcesIconConfig _playerResourcesIconConfig;
        [Inject] private AnalyticsManager _analyticsManager;

        protected override void ShowStart()
        {
            base.ShowStart();

            foreach (var t in _achievementManager.AchievementList)
            {
                var achievement = Target.InstantiateMarketItem();
                var achievementStatus = _achievementManager.GetAchievementsStatus(t.achivement_id);
                achievement.UpdateItemView(t, achievementStatus);

                achievement.ClaimButton.onClick.AddListener(async () =>
                {
                    var rewards = await _achievementManager.AchievementsCompleted(t.achivement_id);
                    if (rewards != null && rewards.Count > 0)
                    {
                        var windowData = new RewardWindowData
                        {
                            Title = "Reward",
                            Description = $"You have successfully finished your goal! Please accept this reward!",
                            NoButtonActive = false,
                            YesButtonText = "CLAIM",
                            //HamstaImage = _playerManager.CurrentCollectionItem.spriteIcon,
                            ResourceImage =  _playerResourcesIconConfig.GetIconSprite(ResourcesType.GoldenBeans),
                            ResourceValue = rewards[0].reward_count,
                        };
                        _uiManager.ForceOpenWindow(WindowType.RewardWindow, windowData);

                        _analyticsManager.UserQuestsEnd(t.achivement_id.ToString());
                    }

                    achievement.UpdateButtonsView(t, _achievementManager.GetAchievementsStatus(t.achivement_id));
                });
            }

            //var poz = Target.AchievementItemTransform.position;
            //Target.AchievementItemTransform.position = new Vector3(poz.x, 0, poz.z);
        }
    }
}