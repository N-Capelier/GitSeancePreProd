using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;

namespace Seance.Testing
{
	/// <summary>
	/// Nico
	/// </summary>
    public class TestingPlayerController : NetworkBehaviour
    {
		[SerializeField] float _moveSpeed = 5f;
		CharacterController _characterController;

		private void Awake()
		{
			_characterController = GetComponent<CharacterController>();
		}

		private void Update()
		{
			if (!IsOwner)
				return;

			float horizontal = Input.GetAxisRaw("Horizontal");
			float vertical = Input.GetAxisRaw("Vertical");
			Vector2 normalizedDirection = new Vector2(horizontal, vertical).normalized;
			Vector3 offset = new Vector3(normalizedDirection.x, Physics.gravity.y, normalizedDirection.y) * (_moveSpeed * Time.deltaTime);

			_characterController.Move(offset);
		}
	}
}
