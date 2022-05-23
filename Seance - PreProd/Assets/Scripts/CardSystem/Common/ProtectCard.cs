using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.BoardManagment;

namespace Seance.CardSystem
{

	/// <summary>
	/// LouisL
	/// </summary>
	[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/ProtectCard", fileName = "New ProtectCard", order = 50)]
	public class ProtectCard : Card
	{
		[SerializeField]
		int armorGain;
		protected override void Use(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{
			TileManager.Instance.ServerRpcPawnGainArmor(caster._pawnID, armorGain);
		}
	}

}
