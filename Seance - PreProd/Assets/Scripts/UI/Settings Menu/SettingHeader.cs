using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Seance.UI.Settings
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class SettingHeader : MonoBehaviour
    {
        [SerializeField] private string _headerName;

        [Space(20)]

        [SerializeField] private TextMeshProUGUI _label;

        #region Unity events

        private void Awake()
        {
            _label.SetText(_headerName);
        }

        #endregion

        #region Public methods
        #endregion

        #region Private methods
        #endregion
    }
}
