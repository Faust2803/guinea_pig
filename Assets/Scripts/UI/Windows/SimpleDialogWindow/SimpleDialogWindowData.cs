using System;
using UnityEngine;

namespace UI.Windows.SimpleDialogWindow
{
    public class SimpleDialogWindowData:WindowData
    {
        public string Title = "SimpleDialogWindow Title";
        public string Description = "SimpleDialogWindow Description";
        public Action YesAction = null;
        public Action NoAction = null;
        public bool NoButtonActive = true;
        public bool YesButtonActive = true;
        public string NoButtonText = "NO";
        public string YesButtonText = "OK";

        public Sprite Image = null;


    }
}