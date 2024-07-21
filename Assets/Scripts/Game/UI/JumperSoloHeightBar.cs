using TMPro;
using UnityEngine;

namespace Game.Jumper
{
    public class JumperSoloHeightBar : MonoBehaviour
    {
        [SerializeField] Vector2[] minAndMaxPoses;
        [SerializeField] RectTransform playerPos;
        [SerializeField] RectTransform smogPos;
        [SerializeField] RectTransform heightScorePos;
        [SerializeField] TextMeshProUGUI moonBank;

        public void SetBank (int amount)
        {
            moonBank.text = NumberFormatter.FormatValue(amount);
        }

        public void SetHeightscoreLevel (int sphereId, float percent)
        {
            var hasRecord = sphereId >= 0 && percent > 0;
            heightScorePos.gameObject.SetActive(hasRecord);
            if (hasRecord)
            {
                var minMax = minAndMaxPoses[sphereId];
                var posY = Mathf.Lerp(minMax.x, minMax.y, percent);
                heightScorePos.anchoredPosition = new Vector2(heightScorePos.anchoredPosition.x, posY);
            }
        }

        public void SetHeightscoreLevel(Vector2 globalProgress)
        {
            SetHeightscoreLevel((int)globalProgress.x, globalProgress.y);
        }

        public void SetPlayerPos(int shpereId, float progress)
        {
            var minMax = minAndMaxPoses[Mathf.Clamp(shpereId, 0, minAndMaxPoses.Length)];
            var posY = Mathf.Lerp(minMax.x, minMax.y, progress);
            playerPos.anchoredPosition = new Vector2(playerPos.anchoredPosition.x, posY);
        }

        public void SetSmogLevel(int sphereId, float progress)
        {
            var minMax = minAndMaxPoses[sphereId];
            var posY = Mathf.Lerp(minMax.x, minMax.y, progress);
            smogPos.anchoredPosition = new Vector2(smogPos.anchoredPosition.x, posY);
        }
    }
}