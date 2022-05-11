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
        GameManager gManager;

		private void Start()
		{
			gManager = GameManager.Instance;
		}

		[ServerRpc(RequireOwnership = false)]
        public void ServerRpcPlayTurn()
		{
			gManager.turnManager.PlayNextTurn();
		}
    }
}
