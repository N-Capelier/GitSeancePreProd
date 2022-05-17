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
		[Header("References")]
		[SerializeField] Transform _cardsParent;
		[SerializeField] GameObject _visualCardPrefab;

		[Header("Params")]
		[SerializeField] List<Card> _deck;
		[SerializeField] List<Card> _hand;
		[SerializeField] List<Card> _discard;

		private void Start()
		{
			for (int i = 0; i < 5; i++)
			{
				DrawCard();
			}
		}

		public void DrawCard()
		{
			if(_deck.Count == 0)
			{
				_deck = _discard;
				_discard = new List<Card>();
			}

			_hand.Add(_deck[0]);
			_deck.RemoveAt(0);

			VisualCard newVisualCard = Instantiate(_visualCardPrefab, _cardsParent).GetComponent<VisualCard>();
			newVisualCard.Init(_hand[_hand.Count - 1], _hand.Count - 1);
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

		public void RefreshCardIndexes()
		{
			VisualCard[] cards = _cardsParent.GetComponentsInChildren<VisualCard>();

			for (int i = 0; i < cards.Length; i++)
			{
				cards[i]._boardIndex = i;
			}
		}
	}
}
