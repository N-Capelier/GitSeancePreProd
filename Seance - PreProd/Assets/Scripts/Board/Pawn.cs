using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>
    
    public class Pawn : MonoBehaviour
    {
        public TileManager.PawnType _thisPawnType = TileManager.PawnType.character;

        public int _pawnID;
        public int _x;
        public int _y;

        public void MovePawnTo(int x, int y)
        {
            Vector3 ground = TileManager.Instance.GetTile(x, y).gameObject.transform.position;

            transform.position = ground + new Vector3(0, TileManager.Instance._tilePrefabs[0].transform.lossyScale.x, 0);
        }
    }
}
