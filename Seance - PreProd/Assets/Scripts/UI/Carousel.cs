using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Seance.UI
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class Carousel : MonoBehaviour
    {
        [SerializeField] private List<string> _options;
        [SerializeField] private int _defaultIndex;
        private int _index;

        [Space(20)]

        [SerializeField] private TextMeshProUGUI label;
        [SerializeField] private Button _leftButton;
        [SerializeField] private Button _rightButton;

        public List<string> Options { get => _options; set => _options = value; }
        public int DefaultIndex { get => _defaultIndex; set => _defaultIndex = value; }
        public int Index { get => _index; set => _index = value; }

        public delegate void OnSettingChange();
        public event OnSettingChange onSettingChange;

        #region Unity events

        private void Start()
        {
            UpdateText();

            if (_index == 0)
            {
                _leftButton.interactable = false;
                _rightButton.interactable = (_options.Count > 1);
            }
            else if(_index == _options.Count-1)
            {
                _leftButton.interactable = true;
                _rightButton.interactable = false;
            }
            else
            {
                _leftButton.interactable = true;
                _rightButton.interactable = true;
            }
        }

        #endregion

        #region Public methods

        public void ActionLeft()
        {
            if(_index > 0)
            {
                _index--;
                UpdateText();
                onSettingChange?.Invoke();

                if (_index <= 0)
                {
                    _leftButton.interactable = false;
                }

                _rightButton.interactable = true;
            }
        }

        public void ActionRight()
        {
            if (_index < _options.Count-1)
            {
                _index++;
                UpdateText();
                onSettingChange?.Invoke();

                if (_index >= _options.Count - 1)
                {
                    _rightButton.interactable = false;
                }

                _leftButton.interactable = true;
            }
        }

        #endregion

        #region Private methods

        private void UpdateText()
        {
            label.SetText($"{_options[_index]}");
        }

        #endregion
    }
}
