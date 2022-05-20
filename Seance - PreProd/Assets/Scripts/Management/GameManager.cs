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
		public LobbyManager _lobby;
		public TurnManager _turnManager;
		public EnemyManager _enemyManager;
		public Camera _defaultCamera;

		[Header("Debug")]
		public TextMeshProUGUI _debugPlayerTurn;

		private void Awake()
		{
			CreateSingleton(true);
		}
	}
}
