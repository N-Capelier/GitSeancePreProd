using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.UI.SettingsMenu
{
    /// <summary>
    /// Edouard
    /// </summary>
    public abstract class SettingBase : MonoBehaviour
    {
        [SerializeField] protected SettingType _settingType;
        [SerializeField] protected string _settingName;

        public SettingType SettingType { get => _settingType; }

        public abstract void SetSettingValue(int value);
        public abstract int GetSettingValue();
    }
}
