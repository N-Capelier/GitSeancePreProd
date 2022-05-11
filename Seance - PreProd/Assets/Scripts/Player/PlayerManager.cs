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

		GameManager gManager;

		public PlayerHand hand;


		private void Start()
		{
			gManager = GameManager.Instance;
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
			GameManager.Instance.lobby.ownedPlayer = this;
		}

		public void EndTurn()
		{
			if (!_isPlaying)
				return;

			GameManager.Instance.debugPlayerTurn.text = "player turn: false";
			_isPlaying = false;

			gManager.turnManager.PlayNextTurn();
		}

		public void StartTurn()
		{
			if(!IsOwner)
				return;

			gManager.debugTurnIndex.text = $"turn index: {gManager.turnManager.TotalTurns % 4}";

			GameManager.Instance.debugPlayerTurn.text = "player turn: true";
			_isPlaying = true;
		}
	}
}
