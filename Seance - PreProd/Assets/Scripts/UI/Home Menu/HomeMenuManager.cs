using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.UI.HomeMenu
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class HomeMenuManager : Singleton<HomeMenuManager>
    {
        private HomeMenuState _state = HomeMenuState.MAIN;

        [Header("UI Sections")]
        [SerializeField] private GameObject _mainMenu;
        [SerializeField] private GameObject _hostMenu;
        [SerializeField] private GameObject _joinMenu;
        [SerializeField] private GameObject _settingsMenu;
        [SerializeField] private GameObject _graphicsSettings;
        [SerializeField] private GameObject _audioSettings;
        [SerializeField] private GameObject _controlsSettings;
        [SerializeField] private GameObject _accessibilitySettings;
        [SerializeField] private GameObject _interfaceSettings;
        [SerializeField] private GameObject _systemSettings;
        [SerializeField] private GameObject _credits;
        [SerializeField] private GameObject _quitAlert;

        #region Unity events

        private void Awake()
        {
            CreateSingleton();
            UpdateSections();
        }

        #endregion

        #region Public methods

        public void ActionStateMain()
        {
            _state = HomeMenuState.MAIN;
            UpdateSections();
        }

        public void ActionStateHost()
        {
            _state = HomeMenuState.HOST_GAME;
            UpdateSections();
        }

        public void ActionStateJoin()
        {
            _state = HomeMenuState.JOIN_GAME;
            UpdateSections();
        }

        public void ActionStateGraphSettings()
        {
            _state = HomeMenuState.GRAPH_SETTINGS;
            UpdateSections();
        }

        public void ActionStateAudioSettings()
        {
            _state = HomeMenuState.AUDIO_SETTINGS;
            UpdateSections();
        }

        public void ActionStateControlsSettings()
        {
            _state = HomeMenuState.CONTROLS_SETTINGS;
            UpdateSections();
        }

        public void ActionStateAccessibilitySettings()
        {
            _state = HomeMenuState.ACCESSIBILITY_SETTINGS;
            UpdateSections();
        }

        public void ActionStateInterfaceSettings()
        {
            _state = HomeMenuState.INTERFACE_SETTINGS;
            UpdateSections();
        }

        public void ActionStateSystemSettings()
        {
            _state = HomeMenuState.SYSTEM_SETTINGS;
            UpdateSections();
        }

        public void ActionStateCredits()
        {
            _state = HomeMenuState.CREDITS;
            UpdateSections();
        }

        public void ActionStateQuitAlert()
        {
            _state = HomeMenuState.QUIT_ALERT;
            UpdateSections();
        }

        public void ActionQuitGame()
        {
            Application.Quit();
        }

        #endregion

        #region Private methods

        private void UpdateSections()
        {
            DisableAllSections();

            switch (_state)
            {
                case HomeMenuState.MAIN:
                    _mainMenu.SetActive(true);
                    break;

                case HomeMenuState.HOST_GAME:
                    _hostMenu.SetActive(true);
                    break;

                case HomeMenuState.JOIN_GAME:
                    _joinMenu.SetActive(true);
                    break;

                case HomeMenuState.GRAPH_SETTINGS:
                    _settingsMenu.SetActive(true);
                    _graphicsSettings.SetActive(true);
                    break;

                case HomeMenuState.AUDIO_SETTINGS:
                    _settingsMenu.SetActive(true);
                    _audioSettings.SetActive(true);
                    break;

                case HomeMenuState.CONTROLS_SETTINGS:
                    _settingsMenu.SetActive(true);
                    _controlsSettings.SetActive(true);
                    break;

                case HomeMenuState.ACCESSIBILITY_SETTINGS:
                    _settingsMenu.SetActive(true);
                    _accessibilitySettings.SetActive(true);
                    break;

                case HomeMenuState.INTERFACE_SETTINGS:
                    _settingsMenu.SetActive(true);
                    _interfaceSettings.SetActive(true);
                    break;

                case HomeMenuState.SYSTEM_SETTINGS:
                    _settingsMenu.SetActive(true);
                    _systemSettings.SetActive(true);
                    break;

                case HomeMenuState.CREDITS:
                    _credits.SetActive(true);
                    break;

                case HomeMenuState.QUIT_ALERT:
                    _quitAlert.SetActive(true);
                    break;

                default:
                    _mainMenu.SetActive(true);
                    break;
            }
        }

        private void DisableAllSections()
        {
            _mainMenu.SetActive(false);
            _hostMenu.SetActive(false);
            _joinMenu.SetActive(false);
            _settingsMenu.SetActive(false);
            _graphicsSettings.SetActive(false);
            _audioSettings.SetActive(false);
            _controlsSettings.SetActive(false);
            _accessibilitySettings.SetActive(false);
            _interfaceSettings.SetActive(false);
            _systemSettings.SetActive(false);
            _credits.SetActive(false);
            _quitAlert.SetActive(false);
        }

        #endregion
    }
}
