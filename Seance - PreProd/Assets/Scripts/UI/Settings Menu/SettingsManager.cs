using Seance.SoundManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.UI.Settings
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

            GetAllSettingsComponents();
            GetAllSettingsValues();
        }

        private void Start()
        {
            SetAllComponentsValues();
            ApplyAllSettings();
        }

        #endregion

        #region Public methods

        public void UpdateSettingValue(SettingType setting, int value)
        {
            if(_settingsValues.ContainsKey(setting))
            {
                _settingsValues[setting] = value;
            }
            else
            {
                _settingsValues.Add(setting, value);
            }

            PlayerPrefs.SetInt($"{setting}", value);
            ApplySetting(setting);
        }

        #endregion

        #region Private methods

        private void GetAllSettingsComponents()
        {
            _settingsComponents = new Dictionary<SettingType, SettingBase>();
            SettingBase[] settingsArray = FindObjectsOfType<SettingBase>();

            foreach (SettingBase sb in settingsArray)
            {
                _settingsComponents.Add(sb.SettingType, sb);
            }
        }

        private void GetAllSettingsValues()
        {
            _settingsValues = new Dictionary<SettingType, int>();

            foreach (SettingType st in _settingsComponents.Keys)
            {
                if (PlayerPrefs.HasKey($"{st}"))
                {
                    int savedValue = PlayerPrefs.GetInt($"{st}");
                    _settingsValues.Add(st, savedValue);
                }
                else
                {
                    if (_settingsComponents.TryGetValue(st, out SettingBase component))
                    {
                        int defaultValue = component.GetSettingDefaultValue();
                        PlayerPrefs.SetInt($"{st}", defaultValue);
                    }
                }
            }
        }

        public void SetAllComponentsValues()
        {
            foreach (SettingType st in _settingsValues.Keys)
            {
                if (_settingsComponents.TryGetValue(st, out SettingBase component))
                {
                    if (_settingsValues.TryGetValue(st, out int value))
                    {
                        component.SetSettingValue(value);
                    }
                }
            }
        }

        private void ApplyAllSettings()
        {
            foreach (SettingType st in _settingsValues.Keys)
            {
                ApplySetting(st);
            }
        }

        private void ApplySetting(SettingType setting)
        {
            switch (setting)
            {
                case SettingType.WINDOW_MODE:
                    UpdateWindowMode();
                    break;

                case SettingType.GENERAL_VOLUME:
                    UpdateVolumes();
                    break;

                case SettingType.MUSIC_VOLUME:
                    UpdateVolumes();
                    break;

                case SettingType.EFFECT_VOLUME:
                    UpdateVolumes();
                    break;

                case SettingType.AMBIANCE_VOLUME:
                    UpdateVolumes();
                    break;

                case SettingType.DIALOGUE_VOLUME:
                    UpdateVolumes();
                    break;

                case SettingType.INTERFACE_VOLUME:
                    UpdateVolumes();
                    break;

                case SettingType.VOICE_METHOD:
                    break;

                case SettingType.LEFT_PLAYER_VOLUME:
                    UpdateVolumes();
                    break;

                case SettingType.RIGHT_PLAYER_VOLUME:
                    UpdateVolumes();
                    break;

                default:
                    break;
            }
        }

        private void UpdateWindowMode()
        {
            if (_settingsValues.TryGetValue(SettingType.WINDOW_MODE, out int value))
            {
                switch (value)
                {
                    case 0:
                        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                        break;

                    case 1:
                        Screen.fullScreenMode = FullScreenMode.Windowed;
                        break;

                    default:
                        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                        break;
                }
            }
        }

        private void UpdateVolumes()
        {
            
        }

        #endregion
    }
}
