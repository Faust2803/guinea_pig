using System;
using UnityEngine;

namespace UI.Windows.RewardWindow
{
    public class RewardWindowData:WindowData
    {
        public string Title = "RewardWindow Title";
        public string Description = "RewardWindow Description";
        public Action YesAction = null;
        public Action NoAction = null;
        public bool NoButtonActive = true;
        public bool YesButtonActive = true;
        public string NoButtonText = "NO";
        public string YesButtonText = "OK";

        public Sprite HamstaImage = null;
        public Sprite ResourceImage = null;
        public int ResourceValue = 1;


    }
}