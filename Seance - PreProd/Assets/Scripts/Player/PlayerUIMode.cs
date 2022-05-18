using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Seance.Player
{
    public class PlayerUIMode : MonoBehaviour
    {
        [SerializeField] Button _endTurnButton;

        public void EnableTurnUI()
		{
            _endTurnButton.interactable = true;
        }

        public void DisableTurnUI()
		{
            _endTurnButton.interactable = false;
		}
    }
}
