using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>
    
    public class Enemy : Pawn
    {
        
        public void TakeAction()
        {
            //TODO : implement different actions
            Pawn closestCharacter = TileManager.Instance.GetClosestPawn(_x, _y, TileManager.PawnType.character);
            float distanceWithClosestCharacter = GetDistanceFromPawn(closestCharacter);
            //TODO : verify if distance is on the great ratio => (one cell = 1f)
            Debug.Log(distanceWithClosestCharacter);

            //if in attack range then attack
            if (distanceWithClosestCharacter <= 1)
            {
                InflictDamageTo(closestCharacter);
            }
            //else, walk to player
            else
            {
                //TODO : implement A*
            }
        }
    }
}
