using TMPro;
using UnityEngine;

namespace Game.Jumper
{
    public class GameScoreCounterUI : MonoBehaviour
    {
        [SerializeField] bool updateOnlyWhenSetup;

        [SerializeField] TextMeshProUGUI peans;
        [SerializeField] TextMeshProUGUI corns;
        [SerializeField] TextMeshProUGUI seeds;
        [SerializeField] TextMeshProUGUI goldenBeans;

        private IPlayerBase target;

        private void Update()
        {
            if (target != null && updateOnlyWhenSetup == false)
            {
                UpdateInfoFromTarget();
            }
        }

        public void SetupTarget(IPlayerBase player)
        {
            target = player;
            if(updateOnlyWhenSetup)
                UpdateInfoFromTarget();

        }

        private void UpdateInfoFromTarget ()
        {
            peans.text = target.PeansCurrency.ToString();
            corns.text = target.CornsCurrency.ToString();
            seeds.text = target.SeedsCurrency.ToString();
            goldenBeans.text = NumberFormatter.FormatValue(target.GoldenBeansCurrency);
        }
    }
}