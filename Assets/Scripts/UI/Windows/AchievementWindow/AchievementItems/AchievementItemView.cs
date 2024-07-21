using DataModels.Achievement;
using Managers;
using TMPro;
using UI.Panels.ToplobbyPanel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.AchievementWindow
{
    public class AchievementItemView : MonoBehaviour
    {
        [SerializeField] private Button _viewButton;
        [SerializeField] private Button _claimButton;
        [SerializeField] private GameObject _doneButton;
        [SerializeField] private TaskItemView _taskItemView;
        [SerializeField] private Transform _tasksParent;
        [SerializeField] private CurrencyItem _rewardInfo;

        [SerializeField] private Color _completedProgressColor;
        [SerializeField] private Color _uncompletedProgressColor;

        public Button ViewButton => _viewButton;
        public Button ClaimButton => _claimButton;
        public CurrencyItem CurrencyItem => _rewardInfo;

        public void UpdateItemView(AchievementModel achievementModel, AchievementStatus achievementStatus)
        {
            //Debug.Log($"Updated {_collectionItemDataModel.collection_name}");
            foreach (var task in achievementModel.tasks)
            {
                //Debug.Log($"Updated {task.task_id}");
                var taskItem = Instantiate(_taskItemView, _tasksParent);
                taskItem.UpdateItemView(task);
            }

            UpdateButtonsView(achievementModel, achievementStatus);
        }

        public void UpdateButtonsView(AchievementModel achievementModel, AchievementStatus achievementStatus)
        {
            var reward = achievementModel.rewards[0];
            _rewardInfo.SetCurrencyData(reward.reward_count, DataModels.PlayerData.ResourcesType.GoldenBeans);
            _rewardInfo.gameObject.SetActive(achievementStatus == AchievementStatus.NotComplete);

            //_viewButton.gameObject.SetActive(!achievementModel.compleated && !areAllTasksCompleted);
            _claimButton.gameObject.SetActive(achievementStatus == AchievementStatus.Completed);
            _doneButton.SetActive(achievementStatus == AchievementStatus.AlredyGeted);
        }
    }
}