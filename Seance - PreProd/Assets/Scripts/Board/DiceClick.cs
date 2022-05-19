using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance
{
    public class DiceClick : MonoBehaviour
    {
        public delegate void OnLeftClick();
        public event OnLeftClick onLeftClick;
        
        public delegate void OnRightClick();
        public event OnRightClick onRightClick;

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
    }
}
