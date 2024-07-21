using System;
using DataModels.PlayerData;
using UnityEngine;
using Zenject;

namespace Managers.Analytics
{
    public class AnalyticsManager:MonoBehaviour
    {
        
        private const string ANALYTICS_FIRST_ENTER = "analytics_first_enter";
        private const string ANALYTICS_FIRST_LOBBY = "analytics_first_lobby";
        private const string ANALYTICS_PLAY_GAME_10 = "analytics_play_game_10";
        private const string ANALYTICS_PLAY_GAME_5 = "analytics_play_game_5";
        private const string ANALYTICS_PLAY_GAME_3 = "analytics_play_game_3";
        
        [Inject] private INetworkManager _networkManager;
        [Inject] private PlayerManager _playerManager;
        
        private AmplitudeAnalyticsManager _amplitude;
        private void Awake()
        {
            _amplitude = new AmplitudeAnalyticsManager(_networkManager);
        }
        private void Start()
        {
            _amplitude.Start();
        }

        private void OnApplicationQuit()
        {
            _amplitude.OnApplicationQuit();
        }

        public void EnterPlayScreen()
        {
            _amplitude.EnterPlayScreen();
        }
        
        public void StartGamesJamperSolo()
        {
            _amplitude.StartGamesJamperSolo();
            _amplitude.UserBalanceStart( _playerManager.Resources[ResourcesType.GoldenBean].Value);
        }
        
        public void PlayGamesJamperSolo()
        {
            _amplitude.PlayGamesJamperSolo();
        }
        
        public void CheckPointJamperSolo(int zoneId)
        {
            var zone = String.Empty;
            switch (zoneId)
            {
                case 0:
                    zone = "troposphere";
                    break;
                case 1:
                    zone = "stratosphere";
                    break;
                case 2:
                    zone = "mesosphere";
                    break;
                case 3:
                    zone = "thermosphere";
                    break;
                case 4:
                    zone = "space";
                    break;
            }
            _amplitude.CheckPointJamperSolo(zone);
        }
        
        public void EndGamesJamperSolo(bool result, int collectedBeans)
        {
            _amplitude.EndGamesJamperSolo();
            if (result)
            {
                _amplitude.MoonGamesJamperSolo(); 
            }
            PlayGamesJamperSoloCount();
            
            _amplitude.UserBalanceEnd(_playerManager.Resources[ResourcesType.GoldenBean].Value + collectedBeans);
            _amplitude.UserBalanceGame(collectedBeans);
        }
        
        private void PlayGamesJamperSoloCount()
        {
            if (!PlayerPrefs.HasKey(ANALYTICS_PLAY_GAME_3))
            {
                _amplitude.PlayGamesJamperSolo_10();
                PlayerPrefs.SetString(ANALYTICS_PLAY_GAME_3, ANALYTICS_PLAY_GAME_3);
                PlayerPrefs.Save();
            }
            else
            {
                if (!PlayerPrefs.HasKey(ANALYTICS_PLAY_GAME_5))
                {
                    _amplitude.PlayGamesJamperSolo_5();
                    PlayerPrefs.SetString(ANALYTICS_PLAY_GAME_5, ANALYTICS_PLAY_GAME_5);
                    PlayerPrefs.Save();
                }
                else
                {
                    if (!PlayerPrefs.HasKey(ANALYTICS_PLAY_GAME_10))
                    {
                        _amplitude.PlayGamesJamperSolo_3();
                        PlayerPrefs.SetString(ANALYTICS_PLAY_GAME_10, ANALYTICS_PLAY_GAME_10);
                        PlayerPrefs.Save();
                    }
                }
            }
        }
        
        public void TutorialFirstScreen()
        {
            _amplitude.TutorialFirstScreen();
        }
        
        public void TutorialSecondScreen()
        {
            _amplitude.TutorialSecondScreen();
        }
        
        public void TutorialEnd()
        {
            _amplitude.TutorialEnd();
        }
        
        public void ViewCollection()
        {
            _amplitude.ViewCollection();
        }
        
        public void ViewProfile()
        {
            _amplitude.ViewProfile();
        }
        
        public void ViewLeaderboard()
        {
            _amplitude.ViewLeaderboard();
        }
        
        public void ViewQuests()
        {
            _amplitude.ViewQuests();
        }
        
        public void Lobby()
        {
            if (!PlayerPrefs.HasKey(ANALYTICS_FIRST_LOBBY))
            {
                _amplitude.FirstLobbyApp();
                PlayerPrefs.SetString(ANALYTICS_FIRST_LOBBY, ANALYTICS_FIRST_LOBBY);
                PlayerPrefs.Save();
            }else{
                _amplitude.Lobby();
            }
        }
        
        public void Tamagotchi(string value)
        {
            _amplitude.Tamagotchi(value);
        }
        
        public void CollectionBuyHamsta(string value)
        {
            _amplitude.CollectionBuyHamsta(value);
        }
        
        public void CollectionEquipmentHamsta()
        {
            _amplitude.CollectionEquipmentHamsta();
        }
        
        public void UserQuestsEnd(string value)
        {
            _amplitude.UserQuestsEnd(value);
        }
        
        public void LostHp(string type, int value)
        {
            if (value == 0)
            {
                _amplitude.EndGamesJumperSoloDiedOf(type);
            }
            else
            {
                _amplitude.PickedupHp(type);
            }
        }
        
        public void AirdropInteraction(string value)
        {
            _amplitude.AirdropInteraction(value);
        }
        
        public void UseAbility(string value)
        {
            _amplitude.UseAbility(value);
        }
    }
}