using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.CardSystem;

namespace Seance.Player
{
	/// <summary>
	/// Nico
	/// </summary>
	public class PlayerHand : MonoBehaviour
	{
		[SerializeField] List<Card> _cards;

		public Card PickCard(int cardIndex)
		{
			return _cards[cardIndex];
		}

		public Card PickRandomCard()
		{
			return _cards[Random.Range(0, _cards.Count)];
		}
	}
}
