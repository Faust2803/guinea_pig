
using Managers;
using System;
using Zenject;

namespace UI.Windows.RewardWindow
{
    public class RewardWindowMediator :BaseWindowMediator<RewardWindowView, RewardWindowData>
    {
        [Inject] private PlayerManager _playerManager;

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

            if (Data.HamstaImage == null)
            {
                Target.HamstaImage.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Target.HamstaImage.transform.parent.gameObject.SetActive(true);
                Target.HamstaImage.sprite = Data.HamstaImage;
            }
            
            if (Data.ResourceImage == null)
            {
                Target.ResourceImage.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                Target.ResourceImage.transform.parent.gameObject.SetActive(true);
                Target.ResourceImage.sprite = Data.ResourceImage;
                Target.ResourceValue = Data.ResourceValue.ToString();
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

            _playerManager.UpdatePlayerData();

            base.CloseStart();
        }

        
    }
}