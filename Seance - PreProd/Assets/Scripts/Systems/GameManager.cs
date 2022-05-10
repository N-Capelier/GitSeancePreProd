using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.Networking;
using Seance.Player;
using TMPro;

namespace Seance.Management
{
	/// <summary>
	/// Nico
	/// </summary>
    public class GameManager : Singleton<GameManager>
    {
		[Header("References")]
		public LobbyManager lobby;
		public TurnManager turnManager;

		[Header("Debug")]
		public TextMeshProUGUI turnIndex; 
		public TextMeshProUGUI playerTurn;

		private void Awake()
		{
			CreateSingleton(true);
		}

		private void Update()
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				foreach (PlayerManager playerManager in lobby.playerManagers)
				{
					playerManager.EndTurn();
				}
			}
		}
	}
}
