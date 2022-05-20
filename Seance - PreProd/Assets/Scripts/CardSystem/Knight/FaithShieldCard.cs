using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.BoardManagment;

namespace Seance.CardSystem
{

	/// <summary>
	/// LouisL
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/FaithShieldCard", fileName = "New FaithShieldCard", order = 50)]
	public class FaithShieldCard : Card
	{
		[SerializeField]
		int armorGain;
		[SerializeField]
		int purifyAmmount;
		protected override void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{
			foreach (Pawn pawn in targetPawns)
			{
				pawn.GainArmor(armorGain);
				if (pawn._pawnType == PawnType.Character)
                {
					CharacterPawn character = pawn as CharacterPawn;
					character.Purify(purifyAmmount);
                }
			}
		}
	}

}
