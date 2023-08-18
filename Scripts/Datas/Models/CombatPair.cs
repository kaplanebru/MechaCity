using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class CombatPair
    {
        public Tower Perpetrator { get; }
        public Tower Victim { get; }

        public bool IsEven { get; }

        public CombatPair(Tower _perpetrator, Tower _victim, bool isEven = false)
        {
            Perpetrator = _perpetrator;
            Victim = _victim;
            IsEven = isEven;
        }
        
        public bool Contains(Tower newTower)
        {
            return Perpetrator == newTower || Victim == newTower;
        }
        public void Combat()
        {
            if (IsEven) return;
            Victim.Descend(Perpetrator.Data.DamagePower);
        }
    }

}
