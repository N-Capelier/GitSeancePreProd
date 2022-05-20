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
		[SyncObject] public readonly SyncList<NetworkConnection> _networkConnections = new SyncList<NetworkConnection>();
		[HideInInspector] public PlayerManager _ownedPlayer;
		[HideInInspector] public NetworkConnection _ownedConnection;

		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);

			if (!IsServer)
				return;
			
			_connectedPlayerCount++;
			_networkConnections.Add(connection);
			_ownedConnection = connection;

			if (_connectedPlayerCount != 3)
				return;

			StartGame();
		}

		void StartGame()
		{
			//Debug.LogWarning("Starting game");
			GameManager.Instance._turnManager.ServerRpcPlayNextTurn();
		}
	}
}