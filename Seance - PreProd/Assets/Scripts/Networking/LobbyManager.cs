using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using FishNet.Object;
using Seance.Player;
using Seance.Management;

namespace Seance.Networking
{
	/// <summary>
	/// Nico
	/// </summary>
	public class LobbyManager : NetworkBehaviour
	{
		[SyncVar] int _connectedPlayerCount = 0;
		[SyncObject] public readonly SyncList<NetworkConnection> connectedPlayers = new SyncList<NetworkConnection>();
		[SyncObject] public readonly SyncList<PlayerManager> playerManagers = new SyncList<PlayerManager>();

		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);

			if (!IsServer)
				return;
			
			_connectedPlayerCount++;
			connectedPlayers.Add(connection);

			if (_connectedPlayerCount != 3)
				return;

			StartCoroutine(StartGame());
		}

		IEnumerator StartGame()
		{
			yield return new WaitUntil(() => playerManagers.Count == 3);
			Debug.LogWarning("Starting game");
			GameManager.Instance.turnManager.PlayNextTurn();
		}
	}
}