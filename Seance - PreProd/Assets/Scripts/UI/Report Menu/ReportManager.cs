using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;

namespace Seance.UI.Report
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class ReportManager : MonoBehaviour
    {
        [SerializeField] private string _formURL;

        [Space(20)]

        [SerializeField] private TMP_InputField _titleField;
        [SerializeField] private TMP_InputField _stepsField;
        [SerializeField] private TMP_InputField _expectedField;
        [SerializeField] private TMP_InputField _resultField;

        #region Public methods

        public void ActionSendReport()
        {
            StartCoroutine(Post());
        }

        #endregion

        #region Private methods

        private IEnumerator Post()
        {
            WWWForm form = new WWWForm();

            form.AddField("entry.627465742", Application.version);
            form.AddField("entry.1943518831", _titleField.text);
            form.AddField("entry.2114999482", _stepsField.text);
            form.AddField("entry.266870464", _expectedField.text);
            form.AddField("entry.532710727", _resultField.text);

            UnityWebRequest request = UnityWebRequest.Post(_formURL, form);
            yield return request.SendWebRequest();

            ClearInputField();
        }

        private void ClearInputField()
        {
            _titleField.text = "";
            _stepsField.text = "";
            _expectedField.text = "";
            _resultField.text = "";
        }

        #endregion
    }
}
