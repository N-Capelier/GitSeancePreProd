using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.UI.SettingsMenu
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class SettingBase : MonoBehaviour
    {
        [SerializeField] protected SettingType _settingType;
        [SerializeField] protected string _settingName;
    }
}
