using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.Common
{
    public class ScrollButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
    {
        [SerializeField] private Scrollbar  _scrollbar;

        [SerializeField] float _speed;
        
        private bool _move = false;
        private float _position;
        
        public void OnPointerDown(PointerEventData eventData)
        {
            _move = true;
            _position = _scrollbar.value;
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            _move = false;
        }

        private void Update()
        {
            if (_move)
            {
                _position += _speed;
                if (_position is < 0 or > 1)
                {
                    _move = false;
                }
                _scrollbar.value  = _position;
            }
        }
    }
}