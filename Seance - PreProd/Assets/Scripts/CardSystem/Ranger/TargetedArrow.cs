using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.BoardManagment;

namespace Seance.CardSystem
{

	/// <summary>
	/// LouisL
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/TargetedArrow", fileName = "New TargetedArrow", order = 50)]
	public class TargetedArrow : Card
	{
		[SerializeField]
		int damage;
		
		protected override void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{
			foreach (Pawn pawn in targetPawns)
			{

				if (pawn._pawnType == PawnType.Character)
					return;
				pawn.TakeDamage(damage);
			}
		}
	}

}
