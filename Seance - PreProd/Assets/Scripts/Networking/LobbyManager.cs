using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object.Synchronizing;
using FishNet.Connection;
using FishNet.Object;
using Seance.Player;
using Seance.Management;
using Seance.BoardManagment;

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
		[HideInInspector] public NetworkConnection _serverConnection;

		bool _initedServerConnection = false;

		public override void OnSpawnServer(NetworkConnection connection)
		{
			base.OnSpawnServer(connection);

			if (!IsServer)
				return;

			if (!_initedServerConnection)
			{
				_initedServerConnection = true;
				_ownedConnection = connection;
				_serverConnection = connection;
			}

			_networkConnections.Add(connection);

			_connectedPlayerCount++;

			if (_connectedPlayerCount != 3)
				return;

			StartCoroutine(StartGame());
		}

		IEnumerator StartGame()
		{
			yield return new WaitForSeconds(2f);

			ServerRpcStartGame();
			yield return new WaitForSeconds(.5f);
			GameManager.Instance._turnManager.ServerRpcPlayNextTurn();
		}

		[ServerRpc(RequireOwnership = false)]
		void ServerRpcStartGame()
		{
			ObserversRpcStartGame();
		}

		[ObserversRpc]
		void ObserversRpcStartGame()
		{
			GameManager.Instance.ipInputMode._ipInput.gameObject.SetActive(false);
			GameManager.Instance.ipInputMode.enabled = false;
			TileManager.Instance.GenerateRoomEditor();
		}
	}
}