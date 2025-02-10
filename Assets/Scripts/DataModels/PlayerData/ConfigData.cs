using System;


namespace DataModels.PlayerData
{
    [Serializable]
    public class ConfigData {
        public int StartLife;
        public int startMoney;
        public int startLevel;
        public int startRecoveryTime;
        public string startName;
        public ShooterConfig shooterConfig;
    }
    
    [Serializable]
    public class ShooterConfig {
        public ShooterCharacterConfig player;
        public ShooterCharacterConfig bot;
    }
    
    [Serializable]
    public class ShooterCharacterConfig {
        public int magazineCapasity;
        public float sootingTimeout;
        public float reloadTimeout;
        public int booletCapasity;
        public int strength;
    }

}
