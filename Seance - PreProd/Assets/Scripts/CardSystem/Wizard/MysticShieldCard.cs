using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.BoardManagment;

namespace Seance.CardSystem
{

	/// <summary>
	/// LouisL
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/MysticShieldCard", fileName = "New MysticShieldCard", order = 50)]
	public class MysticShieldCard : Card
	{
		[SerializeField]
		int armorGain;
		protected override void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{
			foreach (Pawn pawn in targetPawns)
			{
				TileManager.Instance.ServerRpcPawnGainArmor(pawn._pawnID, armorGain);
			}
		}
	}

}
