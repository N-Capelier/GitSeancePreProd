using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Seance.CardSystem
{
    /// <summary>
    /// Nico
    /// </summary>
    //[CreateAssetMenu(menuName = "ScriptableObjects/CardSystem/Card", fileName = "New Card", order = 50)]
    public abstract class Card : ScriptableObject
    {
        public CardType _type;
        public string _title;
        public string _description;
        public int _corruption;
        public bool _hasMovement;

        public abstract void Use(/*targetCell*/);
    }

    public enum CardType
	{
        Common,
        Wizard,
        Knight,
        Ranger
	}

    public enum EffectArea
	{
        Target,
        Self
	}
}