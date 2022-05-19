using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.BoardManagment;

namespace Seance.CardSystem
{

	/// <summary>
	/// LouisL
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/LowerDefenseCard", fileName = "New LowerDefenseCard", order = 50)]
	public class LowerDefenseCard : Card
	{
		[SerializeField]
		int armorDebuff;
		protected override void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{
			foreach (Pawn pawn in targetPawns)
			{
				pawn.DecreaseArmor(armorDebuff);

			}
		}
	}

}
