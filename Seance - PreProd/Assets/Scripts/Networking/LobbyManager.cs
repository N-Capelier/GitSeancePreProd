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
		[SyncObject] public readonly SyncList<NetworkConnection> networkConnections = new SyncList<NetworkConnection>();
		public PlayerManager ownedPlayer;
		public NetworkConnection ownedConnection;

		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);

			if (!IsServer)
				return;
			
			_connectedPlayerCount++;
			networkConnections.Add(connection);
			ownedConnection = connection;

			if (_connectedPlayerCount != 3)
				return;

			StartGame();
		}

		void StartGame()
		{
			Debug.LogWarning("Starting game");
			GameManager.Instance.turnManager.PlayNextTurn();
		}
	}
}