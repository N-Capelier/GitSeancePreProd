using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.CardSystem
{
    /// <summary>
    /// Nico
    /// </summary>
    [CreateAssetMenu(fileName = "New Deck", menuName = "ScriptableObjects/CardSystem/Deck", order = 50)]
    public class Deck : ScriptableObject
    {
        public List<Card> cards;
    }
}