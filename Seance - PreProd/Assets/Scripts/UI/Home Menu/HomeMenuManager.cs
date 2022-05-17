using System.Collections;
using System.Collections.Generic;
using TMPro;
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
        [SerializeField] private CanvasGroup _mainMenu;
        [SerializeField] private CanvasGroup _hostMenu;
        [SerializeField] private CanvasGroup _joinMenu;
        [SerializeField] private CanvasGroup _settingsMenu;
        [SerializeField] private CanvasGroup _graphicsSettings;
        [SerializeField] private CanvasGroup _audioSettings;
        [SerializeField] private CanvasGroup _controlsSettings;
        [SerializeField] private CanvasGroup _accessibilitySettings;
        [SerializeField] private CanvasGroup _interfaceSettings;
        [SerializeField] private CanvasGroup _systemSettings;
        [SerializeField] private CanvasGroup _reportMenu;
        [SerializeField] private CanvasGroup _reportThanks;
        [SerializeField] private CanvasGroup _credits;
        [SerializeField] private CanvasGroup _quitAlert;

        [Space(20)]

        [SerializeField] private TextMeshProUGUI _versionText;

        #region Unity events

        private void Awake()
        {
            CreateSingleton();

            _versionText.text = $"Build {Application.version}";
        }

        private void Start()
        {
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

        public void ActionApplySettings()
        {
            PlayerPrefs.Save();
        }

        public void ActionStateReport()
        {
            _state = HomeMenuState.REPORT_BUG;
            UpdateSections();
        }

        public void ActionStateReportThanks()
        {
            _state = HomeMenuState.REPORT_THANKS;
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
            HideAllSections();

            switch (_state)
            {
                case HomeMenuState.MAIN:
                    ShowSection(_mainMenu);
                    break;

                case HomeMenuState.HOST_GAME:
                    ShowSection(_hostMenu);
                    break;

                case HomeMenuState.JOIN_GAME:
                    ShowSection(_joinMenu);
                    break;

                case HomeMenuState.GRAPH_SETTINGS:
                    ShowSection(_settingsMenu);
                    ShowSection(_graphicsSettings);
                    break;

                case HomeMenuState.AUDIO_SETTINGS:
                    ShowSection(_settingsMenu);
                    ShowSection(_audioSettings);
                    break;

                case HomeMenuState.CONTROLS_SETTINGS:
                    ShowSection(_settingsMenu);
                    ShowSection(_controlsSettings);
                    break;

                case HomeMenuState.ACCESSIBILITY_SETTINGS:
                    ShowSection(_settingsMenu);
                    ShowSection(_accessibilitySettings);
                    break;

                case HomeMenuState.INTERFACE_SETTINGS:
                    ShowSection(_settingsMenu);
                    ShowSection(_interfaceSettings);
                    break;

                case HomeMenuState.SYSTEM_SETTINGS:
                    ShowSection(_settingsMenu);
                    ShowSection(_systemSettings);
                    break;

                case HomeMenuState.REPORT_BUG:
                    ShowSection(_reportMenu);
                    break;

                case HomeMenuState.REPORT_THANKS:
                    ShowSection(_reportThanks);
                    break;

                case HomeMenuState.CREDITS:
                    ShowSection(_credits);
                    break;

                case HomeMenuState.QUIT_ALERT:
                    ShowSection(_quitAlert);
                    break;

                default:
                    ShowSection(_mainMenu);
                    break;
            }
        }

        private void HideAllSections()
        {
            HideSection(_mainMenu);
            HideSection(_hostMenu);
            HideSection(_joinMenu);
            HideSection(_settingsMenu);
            HideSection(_graphicsSettings);
            HideSection(_audioSettings);
            HideSection(_controlsSettings);
            HideSection(_accessibilitySettings);
            HideSection(_interfaceSettings);
            HideSection(_systemSettings);
            HideSection(_reportMenu);
            HideSection(_reportThanks);
            HideSection(_credits);
            HideSection(_quitAlert);
        }

        private void ShowSection(CanvasGroup section)
        {
            section.alpha = 1;
            section.interactable = true;
            section.blocksRaycasts = true;
        }

        private void HideSection(CanvasGroup section)
        {
            section.alpha = 0;
            section.interactable = false;
            section.blocksRaycasts = false;
        }

        #endregion
    }
}
