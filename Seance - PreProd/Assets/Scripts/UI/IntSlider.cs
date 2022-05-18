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
        public int Value { get => _value; set => _value = value; }

        public delegate void OnSettingChange();
        public event OnSettingChange onSettingChange;


        #region Unity events

        private void Start()
        {
            _slider.minValue = _minValue;
            _slider.maxValue = _maxValue;
            _slider.SetValueWithoutNotify(_value);
            _inputField.SetTextWithoutNotify($"{_value}");
        }

        #endregion

        #region Public methods

        public void ActionSliderChanged()
        {
            _value = (int)_slider.value;
            _inputField.SetTextWithoutNotify($"{_value}");

            onSettingChange?.Invoke();

        }

        public void ActionInputChanged()
        {
            _value = int.Parse(_inputField.text);
            _slider.SetValueWithoutNotify(_value);

            onSettingChange?.Invoke();
        }

        #endregion

        #region Private methods
        #endregion
    }
}