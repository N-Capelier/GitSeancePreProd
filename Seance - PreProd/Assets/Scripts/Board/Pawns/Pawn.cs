using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    public enum PawnType
    {
        Character,
        Enemy,
        Total
    }

    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>
    public class Pawn : MonoBehaviour
    {
        //Pascal Case PawnType enum + move it to pawn class
        public PawnType _pawnType = PawnType.Character;

        [Header("Setup")]
        [SerializeField] protected int _baseHealth;
        protected int _currentHealth;
		public int CurrentHealth { get => _currentHealth;}

        //grid related var
        public int _pawnID;
        public int _x;
        public int _y;

        //pawn related var
        protected int _armor = 0;
        public int CurrentArmor { get => _armor; }

        public int _intiativeBase;

        //hard set pawn position to x,y pos on board
        public void ChangePositionTo(int x, int y)
        {
            if (TileManager.Instance.GetTile(x, y) != null)
            {
                Tile origin = TileManager.Instance.GetTile(_x, _y);
                Tile destiation = TileManager.Instance.GetTile(x, y);

                origin._pawnsOnTile.Remove(this);
                origin.UpdatePawnsPositionOnTile();

                Vector3 ground = destiation.gameObject.transform.position;
                transform.position = ground + new Vector3(0, TileManager.Instance._tilePrefabs[0].transform.lossyScale.x, 0);
                destiation._pawnsOnTile.Add(this);
                destiation.UpdatePawnsPositionOnTile();
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

        public virtual void TakeDamage(int damages)
        {
            _currentHealth -= damages;
            if (_currentHealth <= 0) Die();
        }

        public void Heal(int amount)
        {
            if(_currentHealth + amount >= _baseHealth)
            {
                _currentHealth = _baseHealth;
            }
            else
            {
                _currentHealth += amount;
            }
        }

        public virtual void GainArmor(int amount)
		{
            _armor += amount;
		}

        public virtual void DecreaseArmor(int amount)
        {
            if(_armor - amount < 0)
            {
                _armor = 0;
            }
            else
            {
                _armor -= amount;
            }
        }

        public float GetDistanceToPawn(Pawn pawn)
        {
            if (pawn != null)
                return Vector2.Distance(new Vector2(_x, _y), new Vector2(pawn._x, pawn._y));
            throw new System.ArgumentNullException("Other pawn is null");
        }

        public virtual void Die()
        {
            Destroy(gameObject);
        }
    }
}
