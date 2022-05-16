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

            GetAllSettingsComponents();
            GetAllSettingsValues();
            UpdateAllComponants();
            ApplyAllSettings();
        }

        private void Start()
        {
            
        }

        #endregion

        #region Public methods

        public void UpdateSettingValue(SettingType setting, int value)
        {
            _settingsValues[setting] = value;
            PlayerPrefs.SetInt($"{setting}", value);

            ApplySetting(setting);
        }

        #endregion

        #region Private methods

        private void GetAllSettingsComponents()
        {
            _settingsComponents = new Dictionary<SettingType, SettingBase>();
            SettingBase[] settingsArray = FindObjectsOfType<SettingBase>();

            foreach(SettingBase sb in settingsArray)
            {
                _settingsComponents.Add(sb.SettingType, sb);
            }
        }

        private void GetAllSettingsValues()
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

        private void UpdateAllComponants()
        {
            foreach(SettingType st in _settingsValues.Keys)
            {
                if(_settingsComponents.TryGetValue(st, out SettingBase componant))
                {
                    if(_settingsValues.TryGetValue(st,out int value))
                    {
                        componant.SetSettingValue(value);
                    }
                }
            }
        }

        private void ApplyAllSettings()
        {
            foreach(SettingType st in _settingsValues.Keys)
            {
                ApplySetting(st);
            }
        }

        private void ApplySetting(SettingType setting)
        {
            switch (setting)
            {
                case SettingType.DISPLAY_SCREEN:
                    UpdateDisplayScreen();
                    break;
                case SettingType.WINDOW_MODE:
                    UpdateWindowMode();
                    break;
                case SettingType.SCREEN_RESOLUTION:
                    break;
                case SettingType.REFRESH_RATE:
                    break;
                case SettingType.VSYNC:
                    break;
                case SettingType.QUALITY_PRESET:
                    break;
                case SettingType.SHADOW_QUALITY:
                    break;
                case SettingType.ANTIALIASING:
                    break;
                case SettingType.TEXTURE_QUALITY:
                    break;
                case SettingType.EFFECT_QUALITY:
                    break;
                case SettingType.POSTPROCESS_QUALITY:
                    break;
                case SettingType.GENERAL_VOLUME:
                    break;
                case SettingType.MUSIC_VOLUME:
                    break;
                case SettingType.EFFECT_VOLUME:
                    break;
                case SettingType.AMBIANCE_VOLUME:
                    break;
                case SettingType.DIALOGUE_VOLUME:
                    break;
                case SettingType.INTERFACE_VOLUME:
                    break;
                case SettingType.VOICE_METHOD:
                    break;
                case SettingType.VOICE_GLOBAL_VOLUME:
                    break;
                case SettingType.VOICE_LEFT_VOLUME:
                    break;
                case SettingType.VOICE_RIGHT_VOLUME:
                    break;
                case SettingType.OUTPUT_DEVICE:
                    break;
                case SettingType.INPUT_DEVICE:
                    break;
                case SettingType.IMAGE_BRIGHTNESS:
                    break;
                case SettingType.IMAGE_CONTRAST:
                    break;
                case SettingType.SUBTITLE_ENABLE:
                    break;
                case SettingType.SUBTITLE_SIZE:
                    break;
                case SettingType.SUBTITLE_COLOR:
                    break;
                case SettingType.SUBTITLE_BACKGROUND_OPACITY:
                    break;
                case SettingType.SUBTITLE_BACKGROUND_COLOR:
                    break;
                case SettingType.COLORBLIND_FILTER:
                    break;
                case SettingType.COLORBLIND_INTENSITY:
                    break;
                case SettingType.FONT_SIZE:
                    break;
                case SettingType.SYSTEM_LANGUAGE:
                    break;
                case SettingType.ENABLE_DEBUG:
                    break;
                default:
                    break;
            }
        }

        private void UpdateDisplayScreen()
        {
            if (_settingsValues.TryGetValue(SettingType.DISPLAY_SCREEN, out int value))
            {
                PlayerPrefs.SetInt("UnitySelectMonitor", value);
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
                        Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
                        break;

                    case 2:
                        Screen.fullScreenMode = FullScreenMode.Windowed;
                        break;

                    default:
                        Screen.fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                        break;
                }
            }
        }

        #endregion
    }
}
