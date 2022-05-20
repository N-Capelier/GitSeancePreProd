using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.BoardManagment;

namespace Seance.CardSystem
{

	/// <summary>
	/// LouisL
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/ShieldBashCard", fileName = "New ShieldBashCard", order = 50)]
	public class ShieldBashCard : Card
	{

		protected override void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{
			foreach (Pawn pawn in targetPawns)
			{
				pawn.TakeDamage(caster.CurrentArmor);

			}
		}
	}

}
