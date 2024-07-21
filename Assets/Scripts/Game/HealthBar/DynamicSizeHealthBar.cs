using Game.Jumper;
using Managers;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Game
{
    public class DynamicSizeHealthBar : MonoBehaviour
    {
        public bool IsVisibled
        {
            set
            {
                if(TryGetComponent<Image>(out var graphics))
                    graphics.enabled = value;
                transform.GetChild(0).gameObject.SetActive(value);
            }
        }

        [SerializeField] bool hideOnStart = true;
        [SerializeField] Image cellTemplate;
        [SerializeField] float sizePerCell = 40;
        [SerializeField] Vector3 offset = new Vector2(0, 10);
        [SerializeField] Color activeColor = Color.green;
        [SerializeField] Color notActiveColor = Color.clear;

        [Inject] PlayerManager playerData;

        private RectTransform rect;
        private List<Image> health = new List<Image>();
        private JumperSoloPlayer targetPlayer;

        private void Start()
        {
            rect = GetComponent<RectTransform>();

            if(hideOnStart)
                IsVisibled = false;

            for (var x = 0; x < playerData.CurrentCollectionItem.collection_hp; x++)
                PrepareCell();

            SetHealthCount(Mathf.Clamp(playerData.CurrentCollectionItem.collection_current_hp, 1, playerData.CurrentCollectionItem.collection_hp));
        }

        private void FixedUpdate()
        {
            if(targetPlayer != null)
            {
                var screenPoint = Camera.main.WorldToScreenPoint(targetPlayer.transform.position);
                rect.position = screenPoint + offset;
            }
        }

        public void LinkWithPlayer (JumperSoloPlayer player)
        {
            targetPlayer = player;
            targetPlayer.Health.OnValueChanged += OnPlayerHealthChanged;
            SetHealthCount(targetPlayer.Health.Value);
            IsVisibled = true;
        }

        private void OnPlayerHealthChanged (int old, int current)
        {
            SetHealthCount(current);
        }

        private void PrepareCell ()
        {
            var result = Instantiate(cellTemplate, cellTemplate.transform.parent);
            result.gameObject.SetActive(true);
            health.Add(result);
            rect.sizeDelta = new Vector2(health.Count * sizePerCell, rect.sizeDelta.y);
        }

        private void SetHealthCount (int numOfActive)
        {
            for(var x = 0; x < health.Count; x++)
            {
                health[x].color = x < numOfActive ? activeColor : notActiveColor;
            }
        }
    }
}