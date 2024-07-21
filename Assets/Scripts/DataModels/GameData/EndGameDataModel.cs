using System;

namespace DataModels.GameData
{
    [Serializable]
    public class EndGameDataModel
    {
        public int collected_beans;
        public int score;
        public int lost_hp;
        public int game_id = 1;
        public bool moon_reached;
    }
}