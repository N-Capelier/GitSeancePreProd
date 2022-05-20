using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment.Dice
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class DiceClick : MonoBehaviour
    {
        public delegate void OnLeftClick();
        public event OnLeftClick onLeftClick;
        
        public delegate void OnRightClick();
        public event OnRightClick onRightClick;

        #region Unity events

        private void OnMouseOver()
        {
            if(Input.GetMouseButtonDown(0))
            {
                onLeftClick?.Invoke();
            }
            
            if (Input.GetMouseButtonDown(1))
            {
                onRightClick?.Invoke();
            }
        }

        #endregion

        #region Public methods

        #endregion

        #region Private methods

        #endregion
    }
}
