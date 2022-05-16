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
        public int _lifeActu;
        public int _damage;
        public int _armor = 0;
        public int _intiativeBase;

        //hard set pawn position to x,y pos on board
        public void ChangePositionTo(int x, int y)
        {
            if (TileManager.Instance.GetTile(x, y) != null)
            {
                Vector3 ground = TileManager.Instance.GetTile(x, y).gameObject.transform.position;

                transform.position = ground + new Vector3(0, TileManager.Instance._tilePrefabs[0].transform.lossyScale.x, 0);
            }

        }

        //moves pawn "movement" cases toward the tile on position x,y
        public void MoveToward(int x, int y, int movement)
        {
            //TODO : implement A*
            
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

        public float GetDistanceFromPawn(Pawn theOtherpawn)
        {
            if (theOtherpawn != null) return Vector2.Distance(new Vector2(_x, _y), new Vector2(theOtherpawn._x, theOtherpawn._y));
            else return float.PositiveInfinity; //TODO : throw error
        }

        public void Die()
        {
            //when pawn dies
            Destroy(gameObject);
        }


    }
}
