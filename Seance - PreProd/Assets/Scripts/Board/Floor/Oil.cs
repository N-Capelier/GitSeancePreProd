using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    public class Oil : MonoBehaviour
    {
        public int _damage = 1;

        public void DealDamage(Pawn p, int damages = 1)
        {
            TileManager.Instance.ServerRpcPawnTakeDamage(p._pawnID, damages);
        }
    }
}
