using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using DataModels.Achievement;
using DataModels.Rewards;
using Zenject;

namespace Managers
{

    public class AchievementManager
    {
        [Inject] private INetworkManager _networkManager;
        
        public event Action TaskComplete;
        public event Action AchievementComplete;
        
        public AchievementModel[] AchievementList { get; private set; }

        public async UniTask<bool> GetAchievementsList()
        {
            AchievementList = await _networkManager.GetAchievementList();
            return true;
        }

        public async UniTask<bool> CheckAchievementTarget(params AchievementTarget[] targets)
        {
            var result = false;
            if (AchievementList != null)
            {
                for (var j = 0; j < AchievementList.Length; j++)
                {
                    if (AchievementList[j].compleated) continue;
                    for (var  i = 0; i < targets.Length; i++)
                    {
                        result = await CheckTask(AchievementList[j].achivement_id, AchievementList[j].tasks, targets[i]);
                    }
                }
            }
            return result;
        }

        public async UniTask<List<AchievementModel>> ReadyToCompleted(bool completed = false)
        {
            var result = new List<AchievementModel>();
            if (AchievementList != null)
            {
                for (var j = 0; j < AchievementList.Length; j++)
                {
                    if (AchievementList[j].compleated) continue;
                    var counter = 0;
                    for (var i = 0; i < AchievementList[j].tasks.Length; i++)
                    {
                        var task = AchievementList[j].tasks[i];
                        if (task.target_count == task.achieved)
                        {
                            counter++;
                        }
                    }

                    if (counter > 0 && AchievementList[j].tasks.Length == counter)
                    {
                        if (completed)
                        {
                            AchievementList[j].compleated =
                                await _networkManager.AchievementCompleated(AchievementList[j].achivement_id);
                        }
                        result.Add(AchievementList[j]);
                    }
                }
            }
            return result;
        }

        public AchievementStatus GetAchievementsStatus(int achivement_id)
        {
            if (AchievementList != null)
            {
                for (var j = 0; j < AchievementList.Length; j++)
                {
                    if (achivement_id == AchievementList[j].achivement_id)
                    {
                        if (AchievementList[j].compleated) return AchievementStatus.AlredyGeted;
                        
                        var counter = 0;
                        for (var i = 0; i < AchievementList[j].tasks.Length; i++)
                        {
                            var task = AchievementList[j].tasks[i];
                            if (task.target_count == task.achieved)
                            {
                                counter++;
                            }
                        }

                        if (counter > 0 && AchievementList[j].tasks.Length == counter)
                        {
                            return AchievementStatus.Completed;
                        }
                    }
                }
            }

            return AchievementStatus.NotComplete;
        }

        public async UniTask<List<RewardDataModel>> AchievementsCompleted(params int[] achivement_ids)
        {
            var result = new List<RewardDataModel>();
            if (AchievementList != null)
            {
                for (var k = 0; k < achivement_ids.Length; k++)
                {
                    for (var j = 0; j < AchievementList.Length; j++)
                    {
                        if (achivement_ids[k] == AchievementList[j].achivement_id)
                        {
                            if (AchievementList[j].compleated) continue;
                            var t = await CheckTaskCompleat(AchievementList[j]);
                            if (t)
                            {
                                for (var i = 0; i < AchievementList[j].rewards.Length; i++)
                                {
                                    result.Add( AchievementList[j].rewards[i]);
                                }
                               
                            }
                        }
                    }
                }
            }

            if (result.Count > 0)
            {
                AchievementList = await _networkManager.GetAchievementList();
            }
            
            return result;
        }

        private async UniTask<bool> CheckTaskCompleat(AchievementModel achievementModel)
        {
            var counter = 0;
            for (var i = 0; i < achievementModel.tasks.Length; i++)
            {
                if (achievementModel.tasks[i].target_count == achievementModel.tasks[i].achieved)
                {
                    counter++;
                }
            }

            if (counter > 0 && achievementModel.tasks.Length == counter)
            {
                achievementModel.compleated =
                    await _networkManager.AchievementCompleated(achievementModel.achivement_id);
                AchievementComplete?.Invoke();
            }

            return achievementModel.compleated;
        }
        
        private async UniTask<bool> CheckTask(int achievementId, TaskModel[] tasks, AchievementTarget myTarget)
        {
            var result = false;
            foreach (var t in tasks)
            {
                if (t.Type == myTarget.TargetType && t.achieved < t.target_count)
                {
                    //Debug.Log($"<color=orange>Fined Quest = {questTitle} </color>");
                    if (await _networkManager.AchievementTaskCompleat(achievementId, t.task_id, myTarget.Amount))
                    {
                        if (t.achieved + myTarget.Amount <= t.target_count)
                        {
                            t.achieved += myTarget.Amount;
                        }
                        else
                        {
                            t.achieved = t.target_count;
                        }
                        result = true;
                        TaskComplete?.Invoke();
                        //Debug.Log($"<color=green>QuestsManager Task Name =  {myTarget.TargetType.ToString()} target_count = {t.target_count} ompleated = {t.achieved}</color>");
                    }
                }
            }
            return result;
        }
        
        
        
    }

    public enum AchievementStatus
    {
        NotComplete,
        Completed,
        AlredyGeted
    }
}
