using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.BoardManagment;

namespace Seance.CardSystem
{

	/// <summary>
	/// LouisL
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/StruggleCard", fileName = "New StruggleCard", order = 50)]
	public class StruggleCard : Card
	{
		[SerializeField]
		int damageDealt;
		[SerializeField]
		int damageTaken;

		protected override void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{
			caster.TakeDamage(damageTaken);
			foreach (Pawn pawn in targetPawns)
			{
				pawn.TakeDamage(damageDealt);
			}
		}
	}

}
