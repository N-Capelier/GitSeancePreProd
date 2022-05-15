using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Seance.UI.SettingsMenu
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class SettingIntSlider : SettingBase
    {
        [SerializeField] private int _minValue;
        [SerializeField] private int _maxValue;
        [SerializeField] private int _defaultValue;

        [Space(20)]

        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private IntSlider _intSlider;

        #region Unity events

        private void Awake()
        {
            _label.text = _settingName;
            _intSlider.MinValue = _minValue;
            _intSlider.MaxValue = _maxValue;
            _intSlider.DefaultValue = _defaultValue;
        }

        #endregion

        #region Public methods
        #endregion

        #region Private methods
        #endregion
    }
}
