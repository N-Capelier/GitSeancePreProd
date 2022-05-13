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
        [SerializeField] private Animator camAnimator;
        [SerializeField] private CinemachineBrain cameraBrain;
        [SerializeField] private Seance.Player.PlayerManager playerManager;

        public Camera currentCamera;

        [Header("Stats")]
        //total number of cams
        [SerializeField] private int camnbr;

        [SerializeField] private int startCamIndex = 1;
        //current cam
        private int currentCamIndex;



        private void Awake()
        {
            Initialize();
        }

        private void Initialize()
        {
            currentCamIndex = startCamIndex;
            currentCamera = Camera.main;

            SwitchCam();
        }

        private void Update()
        {
            if (!playerManager.IsOwner)
                return;
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
            currentCamIndex += 1;
            currentCamIndex = Mathf.Clamp(currentCamIndex, 0, camnbr - 1);

            SwitchCam();
        }

        private void OnDownArrow()
        {
            currentCamIndex -= 1;
            currentCamIndex = Mathf.Clamp(currentCamIndex, 0, camnbr - 1);

            SwitchCam();
        }

        private void SwitchCam()
        {
            switch (currentCamIndex)
            {
                case 0:
                    camAnimator.Play("HandCamera");
                    break;
                case 1:
                    camAnimator.Play("GlobalCamera");
                    break;
                case 2:
                    camAnimator.Play("BoardCamera");
                    break;
            }
        }


    }
}
