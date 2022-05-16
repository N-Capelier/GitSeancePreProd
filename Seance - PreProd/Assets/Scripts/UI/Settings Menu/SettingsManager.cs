using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.UI.SettingsMenu
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class SettingsManager : Singleton<SettingsManager>
    {
        private Dictionary<SettingType,int> _settingsValues;
        private Dictionary<SettingType,SettingBase> _settingsComponents;   

        #region Unity events

        private void Awake()
        {
            CreateSingleton();

            GetSettingsComponents();
            GetSettingsValues();
        }

        #endregion

        #region Public methods

        

        #endregion

        #region Private methods

        private void GetSettingsComponents()
        {
            _settingsComponents = new Dictionary<SettingType, SettingBase>();
            SettingBase[] settingsArray = FindObjectsOfType<SettingBase>();

            foreach(SettingBase sb in settingsArray)
            {
                _settingsComponents.Add(sb.SettingType, sb);
            }
        }

        private void GetSettingsValues()
        {
            _settingsValues = new Dictionary<SettingType, int>();

            foreach(SettingType st in _settingsComponents.Keys)
            {
                if (PlayerPrefs.HasKey($"{st}"))
                {
                    int savedValue = PlayerPrefs.GetInt($"{st}");
                    _settingsValues.Add(st, savedValue);
                }
                else
                {
                    if(_settingsComponents.TryGetValue(st, out SettingBase component))
                    {
                        int defaultValue = component.GetSettingValue();
                        PlayerPrefs.SetInt($"{st}", defaultValue);
                    }
                }
            }
        }

        private void UpdateComponants()
        {
            foreach(SettingType st in _settingsValues.Keys)
            {
                if(_settingsComponents.TryGetValue(st, out SettingBase componant))
                {
                    if(componant.GetType() == typeof(SettingCarousel))
                    {
                        
                    }
                    else if(componant.GetType() == typeof(SettingIntSlider))
                    {

                    }
                }
            }
        }

        #endregion
    }
}
