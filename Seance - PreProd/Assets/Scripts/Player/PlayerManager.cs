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

		public void EndTurn()
		{
			if (!IsOwner)
				return;

			if (!_isPlaying)
				return;

			GameManager.Instance.playerTurn.text = "player turn: false";
			_isPlaying = false;
			gManager.turnManager.PlayNextTurn();
		}

		[TargetRpc]
		public void RpcStartTurn(NetworkConnection conn)
		{
			GameManager.Instance.playerTurn.text = "player turn: true";
			_isPlaying = true;
			Debug.LogWarning("Starting turn");
		}
	}
}
