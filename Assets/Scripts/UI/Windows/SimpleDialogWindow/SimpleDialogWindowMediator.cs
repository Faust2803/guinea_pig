
using System;

namespace UI.Windows.SimpleDialogWindow
{
    public class SimpleDialogWindowMediator :BaseWindowMediator<SimpleDialogWindowView, SimpleDialogWindowData>
    {

        protected override void ShowStart()
        {
            base.ShowStart();
            Target.YesButton.onClick.AddListener(CloseYes);
            Target.NoButton.onClick.AddListener(CloseNo);
            Target.Title = Data.Title;
            Target.Description = Data.Description;
            Target.YesButton.gameObject.SetActive(Data.YesButtonActive);
            Target.NoButton.gameObject.SetActive(Data.NoButtonActive);
            Target.NoButtonText = Data.NoButtonText;
            Target.YesButtonText = Data.YesButtonText;

            if (Data.Image == null)
            {
                Target.Image.gameObject.SetActive(false);
            }
            else
            {
                Target.Image.gameObject.SetActive(true);
                Target.Image.sprite = Data.Image;
            }
        }

        private  void CloseYes()
        {
            CloseSelf(Data.YesAction);
        }
        
        private  void CloseNo()
        {
            CloseSelf(Data.NoAction);
        }

       
        protected override void CloseStart()
        { 
            Target.YesButton.onClick.RemoveListener(CloseYes);
            Target.NoButton.onClick.RemoveListener(CloseNo);
            base.CloseStart();
        }

        
    }
}