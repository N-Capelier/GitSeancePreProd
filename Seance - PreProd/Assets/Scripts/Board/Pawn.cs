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

        //grid related var
        public int _pawnID;
        public int _x;
        public int _y;

        //pawn related var
        public int _life;
        private int _lifeActu;
        public int _damage;

        public void MovePawnTo(int x, int y)
        {
            if (TileManager.Instance.GetTile(x, y) != null)
            {
                Vector3 ground = TileManager.Instance.GetTile(x, y).gameObject.transform.position;

                transform.position = ground + new Vector3(0, TileManager.Instance._tilePrefabs[0].transform.lossyScale.x, 0);
            }

        }

        public void InflictDamageTo(Pawn p)
        {
            p.ReceiveDamage(_damage);
        }

        public void ReceiveDamage(int damage)
        {
            _lifeActu -= damage;
            if (_lifeActu <= 0) Die();
        }

        public void Die()
        {
            //when pawn dies
            Destroy(gameObject);
        }


    }
}
