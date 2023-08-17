using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class CombatPair
    {
        public Tower Perpetrator { get; }
        public Tower Victim { get; }

        public bool isEven = false;

        public CombatPair(Tower _perpetrator, Tower _victim)
        {
            Perpetrator = _perpetrator;
            Victim = _victim;
        }

        public void Shoot()
        {
            if (isEven) return;
            Perpetrator.Attack(Victim);
        }

        public bool FindTower(Tower newTower)
        {
            return Perpetrator == newTower || Victim == newTower;
        }
    }

}
