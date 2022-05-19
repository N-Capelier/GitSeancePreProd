using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.BoardManagment;

namespace Seance.CardSystem
{

	/// <summary>
	/// LouisL
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/RandomCard", fileName = "New RandomCard", order = 50)]
	public class RandomCard : Card
	{
		[SerializeField]
		int minDamage;
		[SerializeField]
		int maxDamage;

		protected override void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{
			foreach (Pawn pawn in targetPawns)
			{

				pawn.TakeDamage(Random.Range(minDamage,maxDamage));
			}
		}
	}

}
