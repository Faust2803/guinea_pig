using UnityEngine;
using UnityEngine.UI;

namespace Managers.SoundManager.Test
{
    public class PauseMenuTest : MonoBehaviour
    {
        [SerializeField] private Button _open;
        [SerializeField] private Button _close;
        [SerializeField] private GameObject _body;
        [SerializeField] private AudioListener _listener;


        private void Start()
        {
            _open.onClick.AddListener(SetPause);
            _close.onClick.AddListener(EndPause);
        }

        private void SetPause()
        {
            AudioListener.pause = true;
            _body.SetActive(true);
        }

        private void EndPause()
        {
            AudioListener.pause = false;
            _body.SetActive(false);
        }
    }
}