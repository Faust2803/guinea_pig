using Managers;
using UI.Windows;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Common.HamsterPyramid.Test
{
    public class PyramidUiInvoke : MonoBehaviour
    {
        [SerializeField] private Button _btn;

        [Inject] private UiManager _uiManager;

        private void OnEnable()
        {
            _btn.onClick.AddListener(Show);
        }

        private void Show()
        {
            _uiManager.OpenWindow(WindowType.PyramidWindow);
        }

        private void OnDisable()
        {
            _btn.onClick.RemoveListener(Show);
        }
    }
}