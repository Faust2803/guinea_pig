using System;

namespace DataModels.GameData
{
    [Serializable]
    public class StartGameDataModel
    {
        public bool success;
        public int high_score;
        public int moon_bank;
        public int game_id;
    }
}