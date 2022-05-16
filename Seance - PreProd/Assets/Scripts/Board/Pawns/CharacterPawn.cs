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
        public HeroType HeroType { get => _heroType; }


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
    }

    public enum HeroType
	{
        Wizard,
        Knight,
        Ranger
    }
}
