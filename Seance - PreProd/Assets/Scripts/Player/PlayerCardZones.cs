using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Seance.CardSystem;
using Seance.Management;
using Seance.BoardManagment;

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
		GameManager _gManager;

		[Header("Params")]
		[SerializeField] List<Card> _deck;
		[SerializeField] List<Card> _hand;
		[SerializeField] List<Card> _discard;

		List<VisualCard> _visualHand = new List<VisualCard>();

		[HideInInspector] public int _selectedCardIndex = -1;

		private void Start()
		{
			_gManager = GameManager.Instance;
		}

		public void InitZones()
		{
			StartCoroutine(InitZonesCoroutine());
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
			_visualHand.Add(newVisualCard);
		}

		public void SelectCard(int cardIndex)
		{
			if (_selectedCardIndex != -1)
			{
				_visualHand[_selectedCardIndex].Deselect();
			}
			_visualHand[cardIndex].Select();
			_selectedCardIndex = cardIndex;
		}

		public void UseCard(CharacterPawn caster, Tile targetTile, Pawn[] targetPawns)
		{
			if (_selectedCardIndex == -1)
				return;

			_hand[_selectedCardIndex].UseCard(caster, targetTile, targetPawns, _hand[_selectedCardIndex]._corruption);

			DiscardCard(_selectedCardIndex);
			RefreshCardIndexes();
			_selectedCardIndex = -1;
		}

		public void DiscardCard(int cardIndex)
		{
			_discard.Add(_hand[cardIndex]);
			_hand.RemoveAt(cardIndex);

			Destroy(_visualHand[_selectedCardIndex].gameObject);
			_visualHand.RemoveAt(cardIndex);
		}

		public void RefreshCardIndexes()
		{
			VisualCard[] cards = _cardsParent.GetComponentsInChildren<VisualCard>();

			for (int i = 0; i < cards.Length; i++)
			{
				cards[i]._cardIndex = i;
			}
		}

		IEnumerator InitZonesCoroutine()
		{
			WaitForSeconds wait = new WaitForSeconds(.4f);

			yield return new WaitUntil(() => _gManager != null);

			_gManager._lobby._ownedPlayer.AddInteraction();

			yield return new WaitForSeconds(.2f);
			
			for (int i = 0; i < 5; i++)
			{
				DrawCard();
				yield return wait;
			}

			_gManager._lobby._ownedPlayer.RemoveInteraction();
		}
	}
}
