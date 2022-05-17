using UnityEngine;
using TMPro;

namespace Seance.CardSystem
{
    public class VisualCard : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] TextMeshProUGUI _cardTitle;
        [SerializeField] TextMeshProUGUI _cardDescription;
        [SerializeField] TextMeshProUGUI _cardCorruptionCost;

        [Space]
        [HideInInspector] public int _boardIndex;
        public Card _card;

        public void Init(Card card)
		{
            _card = card;
            DisplayCardInformation();
		}

        public void DisplayCardInformation()
		{
            if (_card == null) throw new System.ArgumentNullException("No card found");

            _cardTitle.text = _card._title;
            _cardDescription.text = _card._description;
            _cardCorruptionCost.text = _card._corruption.ToString();
		}
    }
}