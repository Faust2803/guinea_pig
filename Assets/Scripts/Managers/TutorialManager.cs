using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace Managers
{
    public class TutorialManager
    {
        private  const string TUTORIAL_PROGRESS = "TutorialProgress";
        
        private Dictionary<TutorialSteps, bool> _tutorialSteps;

        public TutorialManager()
        {
            GetTutorialSteps();
        }
        
        public bool CheckTutorialSteps(TutorialSteps step)
        {
            return _tutorialSteps.ContainsKey(step);
        }
        
        public void SaveTutorialSteps(TutorialSteps step)
        {
            if (!_tutorialSteps.ContainsKey(step))
            {
                _tutorialSteps.Add(step, true);
            }
            else
            {
                _tutorialSteps[step] = true;
            }
            Save();
        }

        private void GetTutorialSteps()
        {
            if (PlayerPrefs.HasKey(TUTORIAL_PROGRESS))
            {
                var json = PlayerPrefs.GetString(TUTORIAL_PROGRESS);
                _tutorialSteps = JsonConvert.DeserializeObject<Dictionary<TutorialSteps, bool>>(json);
            }
            else
            {
                _tutorialSteps = new Dictionary<TutorialSteps, bool>();
            }
        }
        
        private void Save()
        {
            var json = JsonConvert.SerializeObject(_tutorialSteps);
            PlayerPrefs.SetString(TUTORIAL_PROGRESS, json);
        }
    }
    
    public enum TutorialSteps
    {
        ToTheMoonFirstStart,
        ToTheMoonFirstJump,
        ToTheMoonFirstRight,
        ToTheMoonFirstLeft,
        GetFirstHamsta,
        GameFlow,
        FirstEnterCollection,
    }
    
}