using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using FishNet.Object;

namespace Seance.Networking
{
	/// <summary>
	/// Nico
	/// </summary>
	public class LobbyManager : NetworkBehaviour
	{
		[SyncVar] int _connectedPlayerCount = 0;
		[SyncObject] readonly SyncList<NetworkConnection> _connectedPlayers = new SyncList<NetworkConnection>();

		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);

			if (!IsHost)
				return;

			_connectedPlayerCount++;
			_connectedPlayers.Add(connection);

			if (_connectedPlayerCount != 3)
				return;

			foreach (NetworkConnection conn in _connectedPlayers)
			{
				Debug.LogWarning(conn.ClientId);
			}
			StartGame();
		}

		void StartGame()
		{
			Debug.Log("Starting game");
		}
	}
}