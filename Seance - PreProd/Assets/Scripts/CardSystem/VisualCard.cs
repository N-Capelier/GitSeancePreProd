using UnityEngine;
using TMPro;
using Seance.Management;

namespace Seance.CardSystem
{
    public class VisualCard : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] TextMeshProUGUI _cardTitle;
        [SerializeField] TextMeshProUGUI _cardDescription;
        [SerializeField] TextMeshProUGUI _cardCorruptionCost;
        GameManager _gManager;

        [Space]
        [HideInInspector] public int _cardIndex;
        [HideInInspector] public Card _card;

        public void Init(Card card, int boardIndex)
		{
            _card = card;
            _cardIndex = boardIndex;
            DisplayCardInformation();
            _gManager = GameManager.Instance;
		}

        public void DisplayCardInformation()
		{
            if (_card == null) throw new System.ArgumentNullException("No card found");

            _cardTitle.text = _card._title;
            _cardDescription.text = _card._description;
            _cardCorruptionCost.text = _card._corruption.ToString();
		}

        //UnityEvent OnClick
        public void OnClick()
		{
            _gManager._lobby._ownedPlayer._cardZones._selectedCardIndex = _cardIndex;
		}
    }
}