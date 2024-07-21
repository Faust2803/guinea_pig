
using System.Collections.Generic;
using DataModels.CollectionsData;
using UnityEngine;
using Zenject;

namespace Managers.Analytics
{
    public class AmplitudeAnalyticsManager
    {
        private const string AMPLITUDE_API_KEY = "0d456e15282502835156aa6653d8dc66";
        private const string AMPLITUDE_ANALYTICS_LOG = "Amplitude Analytics send event";
        private INetworkManager _networkManager;


        private Amplitude _amplitude;
        public AmplitudeAnalyticsManager(INetworkManager networkManager)
        {
            _amplitude = Amplitude.getInstance();
            _amplitude.trackSessionEvents(true);
            _amplitude.init(AMPLITUDE_API_KEY);
            _networkManager = networkManager;
        }
        
        public void Start()
        {
            _amplitude.startSession();
        }

        public void OnApplicationQuit()
        {
            _amplitude.endSession();
        }

        public void EnterPlayScreen()
        {
            //_amplitude.logEvent("enter_play_screen");
            var model = new AnalyticDataModel
            {
                event_type = "loading_screen_second"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [loading_screen_second]");
        }
        
        //+
        public void StartGamesJamperSolo()
        {
            //_amplitude.logEvent("start_games_jamper_solo");
            var model = new AnalyticDataModel
            {
                event_type = "start_games_jumper_solo"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [start_games_jumper_solo]");
        }
        
        //+
        public void PlayGamesJamperSolo()
        {
            //_amplitude.logEvent("play_games_jamper_solo");
            var model = new AnalyticDataModel
            {
                event_type = "play_games_jumper_solo"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [play_games_jumper_solo]");
        }
        
        //+
        public void CheckPointJamperSolo(string zone)
        {
            Dictionary<string, object> property = new Dictionary<string, object>();
            property.Add(zone, true);
            
            // _amplitude.logEvent("check_point_jamper_solo", gameProps);
            var model = new AnalyticDataModel
            {
                event_type = "check_point_jumper_solo",
                event_properties = property
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [check_point_jumper_solo] property {zone}");
        }
        
        //+
        public void EndGamesJamperSolo()
        {
            //_amplitude.logEvent("end_games_jamper_solo");
            var model = new AnalyticDataModel
            {
                event_type = "end_games_jumper_solo"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [end_games_jumper_solo]");
        }
        
        //+
        public void MoonGamesJamperSolo()
        {
            //_amplitude.logEvent("moon_games_jamper_solo");
            var model = new AnalyticDataModel
            {
                event_type = "moon_games_jumper_solo"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [moon_games_jumper_solo]");
        }
        
        //+
        public void PlayGamesJamperSolo_10()
        {
            //_amplitude.logEvent("play_games_jamper_solo_10");
            var model = new AnalyticDataModel
            {
                event_type = "play_games_jumper_solo_10"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [play_games_jumper_solo_10]");
        }
        
        //+
        public void PlayGamesJamperSolo_5()
        {
            //_amplitude.logEvent("play_games_jamper_solo_5");
            var model = new AnalyticDataModel
            {
                event_type = "play_games_jumper_solo_5"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [play_games_jumper_solo_5]");
        }
        
        //+
        public void PlayGamesJamperSolo_3()
        {
            //_amplitude.logEvent("play_games_jamper_solo_3");
            var model = new AnalyticDataModel
            {
                event_type = "play_games_jumper_solo_3"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [play_games_jumper_solo_3]");
        }
        
        //+
        public void UserBalanceStart(int value)
        {
            Dictionary<string, object> property = new Dictionary<string, object>();
            property.Add(value.ToString(), true);
            var model = new AnalyticDataModel
            {
                event_type = "user_balance_start",
                event_properties = property
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [user_balance_start] property {value}");
        }
        
        //+
        public void UserBalanceGame(int value)
        {
            Dictionary<string, object> property = new Dictionary<string, object>();
            property.Add(value.ToString(), true);
            var model = new AnalyticDataModel
            {
                event_type = "user_balance_game",
                event_properties = property
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [user_balance_game] property {value}");
        }
        
        //+
        public void UserBalanceEnd(int value)
        {
            Dictionary<string, object> property = new Dictionary<string, object>();
            property.Add(value.ToString(), true);
            var model = new AnalyticDataModel
            {
                event_type = "user_balance_end",
                event_properties = property
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [user_balance_end] property {value}");
        }
        
        //+
        public void TutorialFirstScreen()
        {
            var model = new AnalyticDataModel
            {
                event_type = "tutorial_first_screen"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [tutorial_first_screen]");
        }
        
        //+
        public void TutorialSecondScreen()
        {
            var model = new AnalyticDataModel
            {
                event_type = "tutorial_second_screen"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [tutorial_second_screen]");
        }
        
        //+
        public void TutorialEnd()
        {
            var model = new AnalyticDataModel
            {
                event_type = "tutorial_end"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [tutorial_end]");
        }
        
        public void ViewCollection()
        {
            var model = new AnalyticDataModel
            {
                event_type = "view_collection"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [view_collection]");
        }
        
        public void ViewProfile()
        {
            var model = new AnalyticDataModel
            {
                event_type = "view_profile"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [view_profile]");
        }
        
        public void ViewLeaderboard()
        {
            var model = new AnalyticDataModel
            {
                event_type = "view_leaderboard"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [view_leaderboard]");
        }
        
        public void ViewQuests()
        {
            var model = new AnalyticDataModel
            {
                event_type = "view_quests"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [view_quests]");
        }
        
        public void FirstLobbyApp()
        {
            var model = new AnalyticDataModel
            {
                event_type = "first_lobby_app"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [first_lobby_app]");
        }
        
        public void Lobby()
        {
            var model = new AnalyticDataModel
            {
                event_type = "lobby"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [lobby]");
        }
        
        public void Tamagotchi(string value)
        {
            var model = new AnalyticDataModel();
           
            if (value == "tap_on_hamster")
            {
                model.event_type = value;
                _networkManager.SendAnalytics(model);
                Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [lobby]");
                return;
            }
            
            Dictionary<string, object> property = new Dictionary<string, object>();
            property.Add(value, true);

            model.event_type = "tamagotchi";
            model.event_properties = property;
            
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [tamagotchi] property {value}");
        }
        
        public void CollectionBuyHamsta(string value)
        {
            Dictionary<string, object> property = new Dictionary<string, object>();
            property.Add(value, true);
            var model = new AnalyticDataModel
            {
                event_type = "collection_buy_hamsta",
                event_properties = property
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [collection_buy_hamsta] property {value}");
        }
        
        public void CollectionEquipmentHamsta()
        {
            var model = new AnalyticDataModel
            {
                event_type = "collection_equipment_hamsta"
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [collection_equipment_hamsta]");
        }
        
        public void UserQuestsEnd(string value)
        {
            Dictionary<string, object> property = new Dictionary<string, object>();
            property.Add(value, true);
            var model = new AnalyticDataModel
            {
                event_type = "user_quests_end",
                event_properties = property
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [user_quests_end] property {value}");
        }
        
        //*************************************
        
        public void PickedupHp(string value)
        {
            Dictionary<string, object> property = new Dictionary<string, object>();
            property.Add(value, true);
            var model = new AnalyticDataModel
            {
                event_type = "pickedup_hp",
                event_properties = property
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [pickedup_hp] property {value}");
        }
        
        public void EndGamesJumperSoloDiedOf(string value)
        {
            Dictionary<string, object> property = new Dictionary<string, object>();
            property.Add(value, true);
            var model = new AnalyticDataModel
            {
                event_type = "end_games_jumper_solo_died_of",
                event_properties = property
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [pickedup_hp] property {value}");
        }
        
        public void AirdropInteraction(string value)
        {
            Dictionary<string, object> property = new Dictionary<string, object>();
            property.Add(value, true);
            var model = new AnalyticDataModel
            {
                event_type = "airdrop_interaction",
                event_properties = property
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [airdrop_interaction] property {value}");
        }
        
        public void UseAbility(string value)
        {
            var model = new AnalyticDataModel
            {
                event_type = value
            };
            _networkManager.SendAnalytics(model);
            Debug.Log($" {AMPLITUDE_ANALYTICS_LOG} [{value}]");
        }
    }
}