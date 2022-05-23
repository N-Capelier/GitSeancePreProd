using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FishNet.Transporting.Tugboat;

namespace Seance.Networking
{
    /// <summary>
    /// Nico
    /// </summary>
    public class DefaultCameraUIMode : MonoBehaviour
    {
        public InputField _ipInput;
        [SerializeField] Tugboat _tugboat;

		private void Update()
		{
            _tugboat.SetClientAddress(_ipInput.text);
		}
	}
}
