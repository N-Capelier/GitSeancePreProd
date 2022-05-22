using Seance.CardSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>
    
    public class CharacterPawn : Pawn
    {
        /// <TODO>
        /// - character class (wizard,...)*
        /// - ref to Dice prefab (for Xantin)
        /// - 
        /// </TODO>

        [Header("Params")]
        [SerializeField] HeroType _heroType;
        //public HeroType HeroType { get => _heroType; }

        public void Initialize(int x, int y, int hp, int armor, int initDice, HeroType heroType, int pawnID)
        {
            _x = x;
            _y = y;
            _baseHealth = hp;
            _currentHealth = _baseHealth;
            _armor = armor;
            _intiativeBase = initDice;
            _heroType = heroType;
            _pawnType = PawnType.Character;
        }


		[SerializeField] int _corruption = 0;
		public int Corruption { get => _corruption;}


        public int Corrupt(int amount)
		{
            if (_corruption + amount > 200)
                _corruption = 200;
            else
                _corruption += amount;

            return _corruption;
		}

        public int Purify(int amount)
		{
            if (_corruption - amount < 0)
                _corruption = 0;
            else
                _corruption -= amount;

            return _corruption;
		}
    }

    public enum HeroType
	{
        Wizard,
        Knight,
        Ranger
    }
}
