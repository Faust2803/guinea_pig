using DataModels.MoonBank;
using UnityEngine;

namespace UI.Windows.HamsterOfMoonWindow
{
    public class HamsterOfMoonData : WindowData
    {
        public readonly string PlayerName;
        public readonly int JackpotValue;
        public readonly Sprite IconSprite;
        
        public HamsterOfMoonData(string playerName, int jackpotValue, Sprite iconSprite)
        {
            PlayerName = playerName;
            JackpotValue = jackpotValue;
            IconSprite = iconSprite;
        }

        public HamsterOfMoonData(MoonInfoData moonData)
        {
            if (moonData == null)
            {
                PlayerName = "UknownUser";
                JackpotValue = 0;
                return;
            }

            PlayerName = moonData.UserName;
            JackpotValue = moonData.BankAmount;
        }
    }
}