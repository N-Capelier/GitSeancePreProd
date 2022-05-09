using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.CardSystem
{
    /// <summary>
    /// Nico
    /// </summary>
    [CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/Card", fileName = "New Card", order = 50)]
    public class Card : ScriptableObject
    {
        public string _cardName;
        public int _cost;
    }
}