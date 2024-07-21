using DataModels.Leaderboard;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI.Panels.ToplobbyPanel;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Windows.LeaderboardWindow
{
    public class LeaderboardItemView : MonoBehaviour
    {
        [SerializeField] private GameObject _backgroundDefault;
        [SerializeField] private GameObject _backgroundPlayer;

        [SerializeField] private Color[] _firstPlacesColors;
        [SerializeField] private Image _placeImage;
        [SerializeField] private TextMeshProUGUI _placeText;
        [SerializeField] private TextMeshProUGUI _playerNameText;
        [SerializeField] private CurrencyItem _currencyItem;
        [SerializeField] private Image _userImage;

        internal void UpdateItemView(LeaderboardUsers leaderboardUsersData)
        {           
            _backgroundPlayer.SetActive(leaderboardUsersData.current_position);
            _backgroundDefault.SetActive(!_backgroundPlayer.activeSelf);

            _placeImage.enabled = leaderboardUsersData.position < 4;
            if (_placeImage.enabled)
                _placeImage.color = _firstPlacesColors[leaderboardUsersData.position - 1];

            _placeText.SetText(leaderboardUsersData.position.ToString());
            _playerNameText.SetText(leaderboardUsersData.user_name);

            _currencyItem.SetCurrencyData(leaderboardUsersData.collected_beans, DataModels.PlayerData.ResourcesType.GoldenBean);

            if (leaderboardUsersData.equipment.leaderboard_sprite != null)
            {
                _userImage.gameObject.SetActive(true);
                _userImage.sprite = leaderboardUsersData.equipment.leaderboard_sprite;
            }
            else
            {
                _userImage.gameObject.SetActive(false);
            }
           
        }
    }
}
