using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.Networking;
using Seance.Player;
using TMPro;
using Seance.Enemy;

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
		public EnemyManager enemyManager;

		[Header("Debug")]
		public TextMeshProUGUI debugTurnIndex; 
		public TextMeshProUGUI debugPlayerTurn;

		private void Awake()
		{
			CreateSingleton(true);
		}
	}
}
