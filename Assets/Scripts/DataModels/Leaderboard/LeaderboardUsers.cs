using System;
using UnityEngine;

namespace DataModels.Leaderboard
{
    [Serializable]
    public class LeaderboardData
    {
        public LeaderboardUsers[] data;
       
    }
    
    [Serializable]
    public class LeaderboardUsers
    {
        public int position;
        public bool current_position;
        public string user_name;
        public int total_games;
        public int collected_beans;
        public int lost_hp;
        public int score;
        public int moon_reached;
        public LeaderboardEquipment equipment;
        public int maxscore;

    }
    
    [Serializable]
    public class LeaderboardEquipment
    {
        public int collection_id;
        public string collection_sprite_name;
        public Sprite leaderboard_sprite = null;
    }
}