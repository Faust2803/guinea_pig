using DataModels.Achievement;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.AchievementWindow
{
    public class TaskItemView : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _description;
        [SerializeField] private TextMeshProUGUI _taskProgress;
        [SerializeField] private Slider _progressSlider;

        public void UpdateItemView(TaskModel taskModel)
        {
            //Debug.Log($"Updated {_collectionItemDataModel.collection_name}");
            _description.SetText(taskModel.title);
            _taskProgress.SetText($"{taskModel.achieved}/{taskModel.target_count}");
            
            _progressSlider.value = (float)taskModel.achieved / (float)taskModel.target_count;
        }
    }
}