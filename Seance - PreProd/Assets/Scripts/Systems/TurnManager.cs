using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FishNet.Object.Synchronizing;
using FishNet.Object;

namespace Seance.Management
{
    /// <summary>
    /// Nico
    /// </summary>
    public class TurnManager : NetworkBehaviour
    {
        [SyncVar] int _totalTurns;

        GameManager gManager;

		private void Start()
		{
            gManager = GameManager.Instance;
		}

		public void PlayNextTurn()
        {
            _totalTurns++;
            gManager.turnIndex.text = $"turn index: {_totalTurns}";

            int currentTurnDelta = _totalTurns % 4;

            Debug.LogError($"total turns: {_totalTurns}, turn delta: {currentTurnDelta}");

            if (currentTurnDelta == 0)
			{
                //Play AI turn
                Debug.LogWarning("AI Turn");
			}
            else
			{
                Debug.LogError(gManager.lobby.playerManagers.Count);
                Debug.LogError(gManager.lobby.connectedPlayers.Count);
                Debug.LogError(currentTurnDelta - 1);

                gManager.lobby.playerManagers[currentTurnDelta - 1].RpcStartTurn(gManager.lobby.connectedPlayers[currentTurnDelta - 1]);
			}
        }
    }
}
