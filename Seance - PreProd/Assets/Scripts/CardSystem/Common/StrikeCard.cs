using Seance.BoardManagment;
using UnityEngine;

namespace Seance.CardSystem
{
	/// <summary>
	/// Nico
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/Strike", fileName = "New Strike Card", order = 50)]
	public class StrikeCard : Card
	{
		protected override void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{

		}
	}
}
