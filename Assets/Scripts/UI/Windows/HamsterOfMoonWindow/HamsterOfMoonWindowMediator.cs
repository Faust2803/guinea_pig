namespace UI.Windows.HamsterOfMoonWindow
{
    public class HamsterOfMoonWindowMediator : BaseWindowMediator<HamsterOfMoonWindowView, HamsterOfMoonData>
    {
        protected override void ShowStart()
        {
            if (_data is HamsterOfMoonData data)
                Target.Initialize(data.PlayerName, data.JackpotValue, data.IconSprite);
            else 
                Target.Initialize("username", 0);
            
            if (!WindowView.OpenAnimation)
            {
                Target.SetEndValue();
                ShowEnd();
                return;
            }
            
            Target.Show(ShowEnd);
        }

        protected override void CloseStart()
        {
            if (!WindowView.CloseAnimation)
            {
                Target.SetEndValue();
                CloseFinish();
                return;
            }
            
            Target.Hide(CloseFinish);
        }
    }
}