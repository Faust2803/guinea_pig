using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Misc
{
    public class ButtonSliderView : MonoBehaviour
    {
        public event Action<ButtonSliderView> OnChanged;
        public Vector2 Range => _range;
        public float CurrentValue => _currentValue;
        
        [SerializeField] private Slider _slider;
        
        [Header("Body")] 
        [SerializeField] private Image _frontBody;
        [SerializeField] private Color _colorFrontBody;
        [SerializeField] private Image _backBody;
        [SerializeField] private Color _colorBackBody;

        [Header("Left label")]
        [SerializeField] private TMP_Text _leftFrontLabel;
        [SerializeField] private Color _leftFrontLabelColor;
        [SerializeField] private TMP_Text _leftBackLabel;
        [SerializeField] private Color _leftBackLabelColor;

        [Header("Right label")]
        [SerializeField] private TMP_Text _rightFrontLabel;
        [SerializeField] private Color _rightFrontLabelColor;
        [SerializeField] private TMP_Text _rightBackLabel;
        [SerializeField] private Color _rightBackLabelColor;

        private float _currentValue;
        private Vector2 _range = new Vector2(0f, 1f);
        
        private void OnEnable()
        {
            _slider.onValueChanged.AddListener(ValueChanged);
        }

        private void OnDisable()
        {
            if(_slider != null)
                _slider.onValueChanged.RemoveListener(ValueChanged);
        }

        public void Initialize(float value)
        {
            Reset();
            _currentValue = value;
            _slider.value = _currentValue;
        }

        public ButtonSliderView WithRange(Vector2 range)
        {
             if(range.x >= range.y) return this;
             
            _range = range;
            _slider.minValue = range.x;
            _slider.maxValue = range.y;
            _slider.value = _currentValue;
            // UpdateLabels();
            return this;
        }

        public ButtonSliderView WithLeftLabel(string label)
        {
            _leftFrontLabel.text = label;
            _leftBackLabel.text = label;

            return this;
        }

        public ButtonSliderView WithRightLabel(string label)
        {
            _rightFrontLabel.text = label;
            _rightBackLabel.text = label;
            return this;
        }
        
        private void ValueChanged(float value)
        {
            _currentValue = value;
            OnChanged?.Invoke(this);
            // UpdateLabels();
        }
        
        public ButtonSliderView WithBodyColor(Color front, Color back)
        {
            _frontBody.color = front;
            _backBody.color = back;
            return this;
        }

        public ButtonSliderView WithTextColor(Color frontLeft, Color backLeft, Color frontRight, Color backRight)
        {
            _leftFrontLabel.color = frontLeft;
            _rightFrontLabel.color = frontRight;
            _leftBackLabel.color = backLeft;
            _rightBackLabel.color = backRight;

            return this;
        }

        public void Reset()
        {
            WithLeftLabel(string.Empty);
            WithRightLabel(string.Empty);
            WithTextColor(_leftFrontLabelColor, _leftBackLabelColor, _rightFrontLabelColor, _rightBackLabelColor);
            WithBodyColor(_colorFrontBody, _colorBackBody);
        }
    }
}