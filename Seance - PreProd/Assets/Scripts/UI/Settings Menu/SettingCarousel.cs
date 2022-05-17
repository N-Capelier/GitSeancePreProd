using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Seance.UI.SettingsMenu
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class SettingCarousel : SettingBase
    {
        [SerializeField] private List<string> _options;
        [SerializeField] private int _defaultIndex;

        [Space(20)]

        [SerializeField] private TextMeshProUGUI _label;
        [SerializeField] private Carousel _carousel;

        #region Unity events

        private void Awake()
        {
            _label.text = _settingName;
            _carousel.Options = _options;
            _carousel.DefaultIndex = _defaultIndex;
            _carousel.Index = _defaultIndex;

            _carousel.onSettingChange += UpdateSettingValue;
        }

        #endregion

        #region Public methods

        public override int GetSettingValue()
        {
            return _carousel.Index;
        }

        public override int GetSettingDefaultValue()
        {
            return _carousel.DefaultIndex;
        }

        public override void SetSettingValue(int value)
        {
            _carousel.Index = value;
        }

        #endregion

        #region Private methods
        #endregion
    }
}
