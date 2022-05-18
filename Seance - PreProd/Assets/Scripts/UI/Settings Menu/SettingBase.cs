using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.UI.Settings
{
    /// <summary>
    /// Edouard
    /// </summary>
    public abstract class SettingBase : MonoBehaviour
    {
        [SerializeField] protected SettingType _settingType;
        [SerializeField] protected string _settingName;

        public SettingType SettingType { get => _settingType; }

        public abstract int GetSettingValue();

        public abstract int GetSettingDefaultValue();

        public abstract void SetSettingValue(int value);

        public void UpdateSettingValue()
        {
            SettingsManager.Instance.UpdateSettingValue(_settingType, GetSettingValue());
        }
    }
}
