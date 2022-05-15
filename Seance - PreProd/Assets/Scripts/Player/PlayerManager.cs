using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using Seance.Management;
using FishNet.Connection;

namespace Seance.Player
{
	/// <summary>
	/// Nico
	/// </summary>
	public class PlayerManager : NetworkBehaviour
	{
		bool _isPlaying = false;
		public bool IsPlaying { get => _isPlaying; }

		GameManager _gManager;

		public PlayerHand _hand;


		private void Start()
		{
			_gManager = GameManager.Instance;
		}

		private void Update()
		{
			if (!IsOwner)
				return;

			if (Input.GetKeyDown(KeyCode.Space))
			{
				EndTurn();
			}
		}

		public override void OnStartClient()
		{
			base.OnStartClient();
			if (!IsOwner)
				return;
			GameManager.Instance._lobby._ownedPlayer = this;
		}

		public void EndTurn()
		{
			if (!_isPlaying)
				return;

			GameManager.Instance._debugPlayerTurn.text = "player turn: false";
			_isPlaying = false;

			_gManager._turnManager.ServerRpcPlayNextTurn();
		}

		public void StartTurn()
		{
			if(!IsOwner)
				return;

			_gManager._debugTurnIndex.text = $"turn index: {_gManager._turnManager.TotalTurns % 4}";

			GameManager.Instance._debugPlayerTurn.text = "player turn: true";
			_isPlaying = true;
		}
	}
}
