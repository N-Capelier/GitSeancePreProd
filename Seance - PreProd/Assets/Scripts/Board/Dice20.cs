using Seance.CameraManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Seance.BoardManagment.Dice
{
    /// <summary>
    /// Edouard
    /// </summary>
    public class Dice20 : Singleton<Dice20>
    {
        [Header("Dice")]
        [SerializeField] private int _diceValue = 20;
        [SerializeField] private int _expectedValue = 20;

        [Header("Cheat values")]
        [SerializeField] private float _cheatScore = 0f;
        [SerializeField] private float _cheatScoreDecay = 0.666f;
        [SerializeField] private float _cheatScorePunish = 1f;
        [SerializeField] private float _cheatTolerance = 2f;
        
        [Header("Components")]
        private Animator _animator;
        private DiceClick _diceClick;
        //private CameraShake _mainCameraShake;
        private Vignette _vignetteEffect;

        public int ExpectedValue { get => _expectedValue; set => _expectedValue = value; }
        
        public int DiceValue { get => _diceValue; set => _diceValue = Mathf.Clamp(value, 1, 20); }

        #region Unity events

        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _diceClick = GetComponentInChildren<DiceClick>();
            //_mainCameraShake = Camera.main.GetComponent<CameraShake>();

            _diceClick.onLeftClick += IncreaseDiceValue;
            _diceClick.onRightClick += DecreaseDiceValue;
        }

        public void Init(Volume volumePostprocess)
		{
            _vignetteEffect = (Vignette)volumePostprocess.profile.components[0];
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
            CheckCheat(1);
            DiceValue++;
            UpdateAnimator();
        }

        private void DecreaseDiceValue()
        {
            CheckCheat(-1);
            DiceValue--;
            UpdateAnimator();
        }

        private void CheckCheat(int valueDirection)
        {
            if (valueDirection == Sign(_expectedValue - _diceValue))
            {
                _cheatScore = Mathf.Max(_cheatScore - _cheatScoreDecay, 0f);
            }
            else
            {
                _cheatScore += _cheatScorePunish;
                if(_cheatScore >= _cheatTolerance)
                {
                    CheatFeedback();
                }
            }
        }

        private void CheatFeedback()
        {
            // Don't work: Cinemachine freeze camera position
            //StartCoroutine(_mainCameraShake.Shake(0.2f, 1f, 0.1f, 0.1f));

            StartCoroutine(VignetteEffect(0.2f,0.1f));
        }

        // Mathf.Sign() sucks...
        private int Sign(float number)
        {
            return number < 0 ? -1 : (number > 0 ? 1 : 0);
        }

        private IEnumerator VignetteEffect(float duration,float baseIntensity)
        {
            float elapsed = 0;
            while(elapsed < duration)
            {
                float intensity = baseIntensity * (1 + ((_cheatScore - _cheatTolerance) / 20));
                _vignetteEffect.intensity.overrideState = true;
                _vignetteEffect.intensity.value = Mathf.Lerp(0,intensity,Mathf.Sin((elapsed / duration)*Mathf.PI));

                elapsed += Time.deltaTime;
                yield return 0;
            }
        }

        private void UpdateAnimator()
        {
            _animator.SetInteger("diceValue", _diceValue);
        }

        #endregion
    }
}
