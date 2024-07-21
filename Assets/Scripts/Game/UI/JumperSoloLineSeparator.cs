using TMPro;
using UnityEngine;

namespace Game.Jumper
{
    public class JumperSoloLineSeparator : MonoBehaviour
    {
        [SerializeField]
        private TextMeshPro lineText;

        [SerializeField, Multiline]
        private string lineTextTemplate = "<sprite index=0><sprite index=0><sprite index=0><sprite index=0>{0} ({1}km)";

        [SerializeField]
        private Transform decorParent;

        [SerializeField]
        private ParticleSystem[] particles;

        public void Setup(string layerName, int height, int decorObjectId)
        {
            lineText.text = string.Format(lineTextTemplate, layerName, height);

            for (var x = 0; x < decorParent.childCount; x++)
                decorParent.GetChild(x).gameObject.SetActive(x == decorObjectId);
        }

        public void PlayParticles()
        {
            for (var x = 0; x < particles.Length; x++)
            {
                particles[x].gameObject.SetActive(true);
            }
        }
    }
}