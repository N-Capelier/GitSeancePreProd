using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.BoardManagment;

namespace Seance.CardSystem
{
    /// <summary>
    /// Nico
    /// </summary>
    //[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/Card", fileName = "New Card", order = 50)]
    public abstract class Card : ScriptableObject
    {
        public CardType _type;
        public string _title;
        public string _description;
        public int _corruption;
        public bool _castOnSelf;
        public bool _hasMovement;

        public void UseCard(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns, int corruptionAmount)
		{
            TileManager.Instance.ServerRpcPawnCorrupt(caster._pawnID, corruptionAmount);
            Use(caster, targetTile, targetPawns);
		}

        protected abstract void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns);
    }

    public enum CardType
	{
        Common,
        Wizard,
        Knight,
        Ranger
	}

    public enum EffectArea
	{
        Target,
        Self
	}
}