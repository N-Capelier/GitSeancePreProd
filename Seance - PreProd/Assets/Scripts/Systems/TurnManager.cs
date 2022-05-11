using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object.Synchronizing;
using FishNet.Object;
using FishNet.Connection;
using System;

namespace Seance.Management
{
	/// <summary>
	/// Nico
	/// </summary>
	public class TurnManager : NetworkBehaviour
	{
		[SyncVar(OnChange = nameof(OnTotalTurnsChanged))] int _totalTurns;

		private void OnTotalTurnsChanged(int prev, int next, bool asServer)
		{
			Debug.LogError("total turns changed");
		}

		public int TotalTurns { get => _totalTurns; }

		GameManager gManager;


		private void Start()
		{
			gManager = GameManager.Instance;
		}

		public void PlayNextTurn()
		{
			_totalTurns++;

			int currentTurnDelta = TotalTurns % 4;

			if (currentTurnDelta == 0)
			{
				//Play AI turn
				gManager.enemyManager.ServerRpcPlayTurn();
			}
			else
			{
				ServerRpcStartTurn(gManager.lobby.networkConnections[currentTurnDelta - 1]);
			}
		}

		[ServerRpc(RequireOwnership = false)]
		public void ServerRpcStartTurn(NetworkConnection conn)
		{
			RpcStartTurn(conn);
		}

		[TargetRpc]
		public void RpcStartTurn(NetworkConnection conn)
		{
			gManager.lobby.ownedPlayer.StartTurn();
		}
	}
}
