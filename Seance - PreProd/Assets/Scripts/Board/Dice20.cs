using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment.Dice
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class Dice20 : MonoBehaviour
    {
        private int _diceValue = 20;
        
        private Animator _animator;
        private DiceClick _diceClick;
        
        public int DiceValue { get => _diceValue; set => _diceValue = Mathf.Clamp(value, 1, 20); }

        #region Unity events

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _diceClick = GetComponentInChildren<DiceClick>();

            _diceClick.onLeftClick += IncreaseDiceValue;
            _diceClick.onRightClick += DecreaseDiceValue;
        }

        private void Start()
        {
            UpdateAnimator();
        }

        #endregion

        #region Public methods

        #endregion

        #region Private methods

        private void IncreaseDiceValue()
        {
            DiceValue++;
            UpdateAnimator();
        }

        private void DecreaseDiceValue()
        {
            DiceValue--;
            UpdateAnimator();
        }

        private void UpdateAnimator()
        {
            _animator.SetInteger("DiceValue", _diceValue);
        }

        #endregion
    }
}
