using System;
using DataModels.Rewards;
using UnityEngine.Serialization;

namespace DataModels.Achievement
{
    [Serializable]
    public class AchievementDataModel
    {
        public AchievementModel[] data;
    }
    
    [Serializable]
    public class AchievementModel
    {
        public int achivement_id;
        public string title;
        public string description;
        public string start_date;
        public string end_date;
        public int unlock_level;
        public TaskModel[] tasks;
        public RewardDataModel[] rewards;
        public bool compleated;
        public string achive_type;
    }
    
    [Serializable]
    public class TaskModel
    {
        public int task_id;
        public string title;
        public string description;
        public int target_count;
        
        public string target_type;
        public int achieved;
        public int reward_points = 1;
        public bool compleated;

        public TargetType Type
        {
            get
            {
                Enum.TryParse(target_type, out TargetType myStatus);
                return myStatus;
            }
        }
    }
    
    [Serializable]
    public class TaskCompleatedModel
    {
        public int achivement_id;
        public int task_id;
        public int counter;
    }
    
    [Serializable]
    public class AchievementCompleatedModel
    {
        public int achivement_id;
    }
    
    [Serializable]
    public class AchievementRewardDataModel
    {
        public bool success;
        public string message;
        public RewardDataModel[] rewards;
    }
    
    public class AchievementTarget 
    {
        public TargetType TargetType { get; private set; }
        public int Amount{ get; private set; }

        public  AchievementTarget(TargetType targetType, int amount = 1)
        {
            TargetType = targetType;
            Amount = amount;
        }
    }
    
    public enum TargetType
    {
        PlayGame, //+
        UseCorn, //+
        UsePeas, //+
        UseSeed, //+
        Achieve2Zone, //+
        Achieve3Zone, //+
        Achieve4Zone, //+
        Achieve5Zone, //+
        AchieveMoon, //+
        BuyBeans,
        EarnBeans, //+
        BuyHamster, //+
        CollectHamsters, //+
        FeedHamster, //+
        SpendBeans, //++
        DieXTimes, //+

    }
    
    public enum AchiveType
    {
        Permanent
    }
    
    
}