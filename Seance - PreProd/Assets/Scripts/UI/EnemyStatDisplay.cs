using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.BoardManagment;
using TMPro;

namespace Seance.UI
{
    /// <summary>
    /// Louis L
    /// </summary>
    public class EnemyStatDisplay : MonoBehaviour
    {

        [SerializeField]
        private TextMeshPro _displayHealth;
        [SerializeField]
        private TextMeshPro _displayArmor;


        public void ActualizeDisplayHealth(int currentHealth)
        {
            _displayHealth.text = currentHealth.ToString();
        }
        public void ActualizeDisplayArmor(int currentArmor)
        {
            _displayHealth.text = currentArmor.ToString();
        }
        public void ActualizeAll(int currentHealth,int currentArmor)
        {
            _displayHealth.text = currentArmor.ToString();
            _displayHealth.text = currentHealth.ToString();
        }
    }
}
