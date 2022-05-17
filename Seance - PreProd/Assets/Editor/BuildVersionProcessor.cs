using System;
using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine;

namespace Seance.Build
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class BuildVersionProcessor : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        private char[] _chars = "0123456789abcdefghijklmnopqrstuvwxyz".ToCharArray();

        #region Unity events

        public void OnPreprocessBuild(BuildReport report)
        {
            DateTime timestampStart = new DateTime(2022, 4, 11, 0, 0, 0, DateTimeKind.Utc);
            int timestamp = (int)(DateTime.UtcNow - timestampStart).TotalSeconds;
            string newVersion = GenerateHashTime(timestamp);
            PlayerSettings.bundleVersion = $"{newVersion}";
        }

        #endregion

        #region Private methods

        private string GenerateHashTime(int timestamp)
        {
            string hash = "";

            while(timestamp > 0)
            {
                hash += _chars[timestamp % _chars.Length];
                timestamp /= _chars.Length;
            }

            return hash;
        }

        #endregion
    }
}
