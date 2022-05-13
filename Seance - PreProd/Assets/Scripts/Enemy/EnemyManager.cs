using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object;
using Seance.Management;

namespace Seance.Enemy
{
	/// <summary>
	/// Nico
	/// </summary>
    public class EnemyManager : NetworkBehaviour
    {
        GameManager _gManager;

		private void Start()
		{
			_gManager = GameManager.Instance;
		}

		[ServerRpc(RequireOwnership = false)]
        public void ServerRpcPlayTurn()
		{
			_gManager._turnManager.PlayNextTurn();
		}
    }
}
