using Common.HamsterPyramid;
using Managers.Analytics;
using UI.Panels;
using UnityEngine;
using Zenject;

namespace UI.Windows.PyramidWindow
{
    public class PyramidWindowMediator : BaseWindowMediator<PyramidWindowView, WindowData>
    {
        [Inject] private HamsterPyramidController _pyramidController;
        [Inject] private AnalyticsManager _analyticsManager;

        protected override void ShowStart()
        {
            _uiManager.ClosePanel(PanelType.TopLobbyPanel);
            _uiManager.ClosePanel(PanelType.BottomLobbyPanel);
            _uiManager.ClosePanel(PanelType.TamagochiPanel);

            var durationShow = _pyramidController.Show();
            
            var currentColor = _pyramidController.CurrentColorBg;
            currentColor.a = Target.Background.color.a;

            Target.Background.color = currentColor;
            Target.Show(durationShow);
            
            Target.ToMoonScrollBtn.Initialize(CheckMoonVisible);
            Target.ToMoonScrollBtn.OnClick += ScrollToMoon;
            Target.ToMoonScrollBtn.Show();
            
            Target.ToHamsterScrollBtn.Initialize(CheckTargetHamster);
            Target.ToHamsterScrollBtn.OnClick += ScrollToHamster;
            Target.ToHamsterScrollBtn.Show();
        }

        protected override void CloseStart()
        {
            var durationHide = _pyramidController.Hide(false, CloseFinish);
            
            var currentColor = _pyramidController.CurrentColorBg;
            currentColor.a = Target.Background.color.a;
            
            Target.Background.color = currentColor;
            Target.Hide(durationHide);
            
            Target.ToMoonScrollBtn.Deinitialize();
            Target.ToMoonScrollBtn.OnClick -= ScrollToMoon;
            Target.ToMoonScrollBtn.Hide();
            Target.ToHamsterScrollBtn.Deinitialize();
            Target.ToHamsterScrollBtn.OnClick -= ScrollToHamster;
            Target.ToHamsterScrollBtn.Hide();
        }

        protected override void CloseFinish()
        {
            _uiManager.OpenPanel(PanelType.TopLobbyPanel);
            _uiManager.OpenPanel(PanelType.BottomLobbyPanel);
            _uiManager.OpenPanel(PanelType.TamagochiPanel);

            base.CloseFinish();
        }

        private bool CheckMoonVisible()
        {
            var moonBounds = _pyramidController.MoonBounds;
            var cameraPos = _pyramidController.CameraInfo.CurrentPosition;
            var cameraBounds = _pyramidController.CameraInfo.CameraBounds;

            return moonBounds.center.y > cameraPos.y && !cameraBounds.InSight(moonBounds.center);
        }

        private bool CheckTargetHamster()
        {
            var hamsterBounds = _pyramidController.TargetHamsterBound;
            var cameraPos = _pyramidController.CameraInfo.CurrentPosition;
            var cameraBounds = _pyramidController.CameraInfo.CameraBounds;
            return hamsterBounds.center.y < cameraPos.y && !cameraBounds.InSight(hamsterBounds.center);
        }

        private void ScrollToMoon()
        {
            Target.ToMoonScrollBtn.Hide();
            if(!Target.ToHamsterScrollBtn.IsVisible)
                Target.ToHamsterScrollBtn.Show();
            _pyramidController.ShowMoon(false, () => Target.ToMoonScrollBtn.Show());

            _analyticsManager.AirdropInteraction("scroll_up");
        }

        private void ScrollToHamster()
        {
            Target.ToHamsterScrollBtn.Hide();
            if(!Target.ToMoonScrollBtn.IsVisible)
                Target.ToMoonScrollBtn.Show();
            _pyramidController.ShowTargetHamster(false, () => Target.ToHamsterScrollBtn.Show());
        }
    }
}