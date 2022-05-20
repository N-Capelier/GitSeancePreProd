using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

namespace Seance.CameraManagement
{
    /// <summary>
    /// [LouisL]
    /// </summary>
    public class CameraSwitch : MonoBehaviour
    {
        [Header("Requirements")]
        [SerializeField] private Animator _camAnimator;
        [SerializeField] private CinemachineBrain _cameraBrain;
		[SerializeField] public Camera _camera;

        [Header("Stats")]
        [SerializeField] private int _camCount;
        [SerializeField] private int _startCamIndex = 1;
        private int _currentCamIndex;

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            _currentCamIndex = _startCamIndex;

            SwitchCam();
        }

        private void Update()
        {
            GetInput();
        }

        private void GetInput()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.Z))
            {
                OnUpArrow();
            }

            if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
            {
                OnDownArrow();
            }
        }

        private void OnUpArrow()
        {
            _currentCamIndex += 1;
            _currentCamIndex = Mathf.Clamp(_currentCamIndex, 0, _camCount - 1);

            SwitchCam();
        }

        private void OnDownArrow()
        {
            _currentCamIndex -= 1;
            _currentCamIndex = Mathf.Clamp(_currentCamIndex, 0, _camCount - 1);

            SwitchCam();
        }

        private void SwitchCam()
        {
            switch (_currentCamIndex)
            {
                case 0:
                    _camAnimator.Play("HandCamera");
                    break;
                case 1:
                    _camAnimator.Play("GlobalCamera");
                    break;
                case 2:
                    _camAnimator.Play("BoardCamera");
                    break;
            }
        }


    }
}
