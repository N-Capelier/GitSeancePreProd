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
		int _totalTurns;
		public int TotalTurns { get => _totalTurns; }

		GameManager _gManager;

		private void Start()
		{
			_gManager = GameManager.Instance;
		}

		[ServerRpc(RequireOwnership = false)]
		public void ServerRpcPlayNextTurn()
		{
			_totalTurns++;

			ObserverRpcUpdateTurn(_totalTurns);

			int currentTurnDelta = TotalTurns % 4;

			if (currentTurnDelta == 0)
			{
				//Play AI turn
				_gManager._enemyManager.ServerRpcPlayTurn();
			}
			else
			{
				ServerRpcStartTurn(_gManager._lobby._networkConnections[currentTurnDelta - 1]);
			}
		}

		[ObserversRpc]
		void ObserverRpcUpdateTurn(int totalTurns)
		{
			_totalTurns = totalTurns;
		}

		[ServerRpc(RequireOwnership = false)]
		public void ServerRpcStartTurn(NetworkConnection conn)
		{
			RpcStartTurn(conn);
		}

		[TargetRpc]
		public void RpcStartTurn(NetworkConnection conn)
		{
			_gManager._lobby._ownedPlayer.StartTurn();
		}
	}
}
