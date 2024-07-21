using Managers;
using Managers.Analytics;
using Zenject;

namespace UI.Windows.Tutorial
{
    public class HowToProgressTutorialMediator :BaseWindowMediator<HowToProgressTutorialView, WindowData>
    {
        [Inject] private TutorialManager _tutorialManager;
        [Inject] private AnalyticsManager _analyticsManager;
        
        protected override void CloseStart()
        { 
            base.CloseStart();
            _tutorialManager.SaveTutorialSteps(TutorialSteps.GameFlow);
            _analyticsManager.TutorialEnd();
        }

    }
}