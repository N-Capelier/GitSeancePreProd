using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Seance.UI
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class IntSlider : MonoBehaviour
    {
        [SerializeField] private int _minValue;
        [SerializeField] private int _maxValue;
        [SerializeField] private int _defaultValue;
        private int _value;

        [Space(20)]

        [SerializeField] private Slider _slider;
        [SerializeField] private TMP_InputField _inputField;

        public int MinValue { get => _minValue; set => _minValue = value; }
        public int MaxValue { get => _maxValue; set => _maxValue = value; }
        public int DefaultValue { get => _defaultValue; set => _defaultValue = value; }

        #region Unity events

        private void Start()
        {
            _value = _defaultValue;
            _slider.minValue = _minValue;
            _slider.maxValue = _maxValue;
            _slider.value = _value;
            _inputField.text = $"{_slider.value}";
        }

        #endregion

        #region Public methods

        public void ActionSliderChanged()
        {
            _inputField.text = $"{_slider.value}";
        }

        public void ActionInputChanged()
        {
            _slider.value = int.Parse(_inputField.text);
        }

        #endregion

        #region Private methods
        #endregion
    }
}
