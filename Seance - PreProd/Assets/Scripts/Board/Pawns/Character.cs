using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.BoardManagment
{
    /// <summary>
    /// This script was created by Julien haigron
    /// </summary>
    
    public class Character : Pawn
    {
        /// <TODO>
        /// - character class (wizard,...)*
        /// - ref to Dice prefab (for Xantin)
        /// - 
        /// </TODO>


        public void Initialize(int hp, int armor, int initDice, /*CharacterType characterType,*/ int pawnID)
        {
            _life = hp;
            _lifeActu = _life;
            _armor = armor;
            _intiativeBase = initDice;
            //TODO : to implement from Nico's version
            //_characterType = characterType;
            _thisPawnType = TileManager.PawnType.character;
        }


    }
}
