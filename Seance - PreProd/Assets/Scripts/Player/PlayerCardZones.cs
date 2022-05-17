using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.CardSystem;

namespace Seance.Player
{
	/// <summary>
	/// Nico
	/// </summary>
	public class PlayerCardZones : MonoBehaviour
	{
		[SerializeField] List<Card> _deck;
		[SerializeField] List<Card> _hand;
		[SerializeField] List<Card> _discard;

		public void DrawCard()
		{
			if(_deck.Count == 0)
			{
				_deck = _discard;
				_discard = new List<Card>();
			}

			_hand.Add(_deck[0]);
			_deck.RemoveAt(0);
		}

		public void UseCard(int cardIndex/*, targetCell*/)
		{
			if (cardIndex < 0 || cardIndex >= _hand.Count)
				throw new System.ArgumentOutOfRangeException("Card index out of range");

			_hand[cardIndex].Use();

			DiscardCard(cardIndex);
		}

		public void DiscardCard(int cardIndex)
		{
			_discard.Add(_hand[cardIndex]);
			_hand.RemoveAt(cardIndex);
		}

	}
}
