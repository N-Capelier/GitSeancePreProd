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

			int cardIndex = Random.Range(0, _deck.Count);

			_hand.Add(_deck[cardIndex]);
			_deck.RemoveAt(cardIndex);
		}

		public void UseCard(int cardIndex/*, targetCell*/)
		{
			if (cardIndex < 0 || cardIndex >= _hand.Count)
				throw new System.ArgumentOutOfRangeException("Card index out of range");

			_hand[cardIndex].Use();

			_discard.Add(_hand[cardIndex]);
			_hand.RemoveAt(cardIndex);
		}
	}
}
