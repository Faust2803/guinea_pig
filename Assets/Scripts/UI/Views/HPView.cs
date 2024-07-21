using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Views
{
    public class HPView : MonoBehaviour
    {
        [SerializeField] private Color _disabledColor;
        [SerializeField] private Color _activeColor;

        [SerializeField] private Image[] _hpSlotImages;

        internal void UpdatePreview(int currentHpCount)
        {
            for (int i = 0; i < currentHpCount; i++)
                _hpSlotImages[i].color = _activeColor;
        }

        internal void ResetHPSlots()
        {
            for (int i = 0; i < _hpSlotImages.Length; i++)
                _hpSlotImages[i].color = _disabledColor;
        }
    }
}
