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
        //Pascal Case PawnType enum + move it to pawn class
        public TileManager.PawnType _pawnType = TileManager.PawnType.character;

        [Header("Setup")]
        [SerializeField] int _baseHealth;
        private int _currentHealth;
		public int CurrentHealth { get => _currentHealth;}

        //grid related var
        public int _pawnID;
        public int _x;
        public int _y;

        //pawn related var
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

        public void DealDamage(Pawn p, int damages)
        {
            p.TakeDamage(damages);
        }

        public void TakeDamage(int damages)
        {
            _currentHealth -= damages;
            if (_currentHealth <= 0) Die();
        }

        public float GetDistanceToPawn(Pawn pawn)
        {
            if (pawn != null)
                return Vector2.Distance(new Vector2(_x, _y), new Vector2(pawn._x, pawn._y));
            throw new System.ArgumentNullException("Other pawn is null");
        }

        public void Die()
        {
            Destroy(gameObject);
        }
    }
}
