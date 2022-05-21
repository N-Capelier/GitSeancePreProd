using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.UI;
using System.Linq;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>

    public class EnemyPawn : Pawn
    {
        public EnemyType _thisEnemyType = EnemyType.enemy1;

        [SerializeField] int _damages;
        [SerializeField] EnemyStatDisplay statDisplay;
        //TODO : function that itinitalise every var for TileManager.SpawnPawns
        //do the same for Character & Tile
        public void Initialize(int x, int y, int hp, int armor, int initDice, EnemyType enemyType, int pawnID)
        {
            _x = x;
            _y = y;
            _baseHealth = hp;
            _currentHealth = _baseHealth;
            _armor = armor;
            _intiativeBase = initDice;
            _thisEnemyType = enemyType;
            _pawnType = PawnType.Enemy;
            statDisplay.ActualizeAll(_currentHealth, _armor);
        }


        public void TakeAction()
        {
            //get closes pawn of type 'Character'
            Pawn closestCharacter = TileManager.Instance.GetClosestPawn(_x, _y, PawnType.Character);
            float distanceWithClosestCharacter = GetDistanceToPawn(closestCharacter);
            if (distanceWithClosestCharacter == float.PositiveInfinity)
            {
                //ERROR : no other pawn of choosen type on board
                return;
            }
            //TODO : verify if distance is on the great ratio => (one cell = 1f)
            Debug.Log($"Enemy distance to closest character: {distanceWithClosestCharacter}");

            //wich enemy type ?
            switch (_thisEnemyType)
            {
                //IA based on Enemy Type
                case EnemyType.enemy1:
                    //if in attack range then attack
                    if (distanceWithClosestCharacter < 4)
                    {
                        //inflict damage on 4 cases around pawn
                        Pawn p1 = TileManager.Instance.GetPawnOn(_x - 1, _y); //left
                        Pawn p2 = TileManager.Instance.GetPawnOn(_x + 1, _y); //right
                        Pawn p3 = TileManager.Instance.GetPawnOn(_x, _y - 1); //up
                        Pawn p4 = TileManager.Instance.GetPawnOn(_x, _y + 1); //down

                        if (p1 != null)
                        {
                            DealDamage(p1, 2);
                        }
                        if (p2 != null)
                        {
                            DealDamage(p2, 2);
                        }
                        if (p3 != null)
                        {
                            DealDamage(p3, 2);
                        }
                        if (p4 != null)
                        {
                            DealDamage(p4, 2);
                        }

                        DealDamage(closestCharacter, 2);
                    }
                    //else, walk to player
                    else
                    {
                        MoveToward(closestCharacter._x, closestCharacter._y, 3);
                    }
                    break;
                case EnemyType.enemy2:
                    //if in attack range then attack
                    if (distanceWithClosestCharacter < 4)
                    {
                        DealDamage(closestCharacter, 2);
                    }
                    //else, walk to player
                    else
                    {
                        //TODO : implement A*
                        MoveToward(closestCharacter._x, closestCharacter._y, 3);
                    }
                    break;
            }
        }
        public override void TakeDamage(int damages)
        {
            base.TakeDamage(damages);
            statDisplay.ActualizeDisplayHealth(_currentHealth);
        }
        public override void DecreaseArmor(int amount)
        {
            base.DecreaseArmor(amount);
            statDisplay.ActualizeDisplayArmor(_armor);
        }

        [ContextMenu("Die()")]
        public override void Die()
        {
            //Calcul que je veux - Tester des trucs.
            int counter = 0;

            List<Pawn> pawnList = TileManager.Instance._pawnsInScene.ToList();
            pawnList.Remove(this);
            TileManager.Instance._pawnsInScene = pawnList.ToArray();

            for (int i = 0; i < TileManager.Instance._pawnsInScene.Length; i++)
            {
                if (TileManager.Instance._pawnsInScene[i] != null)
                {
                    if (TileManager.Instance._pawnsInScene[i]._pawnType == PawnType.Enemy)
                    {
                        counter++;
                    }
                }
            }

            if(counter == 0)
            {
                TileManager.Instance.NextRoomFeedback();
            }

            base.Die();
        }
    }


    public enum EnemyType
    {
        enemy1,
        enemy2,
        total
    }
}
