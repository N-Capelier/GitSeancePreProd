using Seance.BoardManagment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.CardSystem
{
	/// <summary>
	/// Nico
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/TestCard", fileName = "New TestCard", order = 50)]
	public class TestCard : Card
	{
		public override void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{


		}
	}
}
