using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Jumper
{
    public class GameControllButton : MonoBehaviour,
        IPointerDownHandler,
        IPointerUpHandler,
        IPointerExitHandler,
        IPointerEnterHandler
    {
        [SerializeField] float targetAxisValue = 1;
        public Action OnClicked;
        private bool isPressed;

        public float Axis
        {
            get
            {
                return isPressed ? targetAxisValue : 0;
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            isPressed = true;
            OnClicked?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            isPressed = false;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (isPressed)
                isPressed = false;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if(isPressed == false && (Input.GetMouseButton(0) || Input.touchCount > 0))
                isPressed = true;
        }
    }
}