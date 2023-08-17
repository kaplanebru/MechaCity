using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    public class PassivePair : CombatPair
    {
        private Tower[] EvenTowers = new Tower[2];
        public PassivePair(Tower _perpetrator, Tower _victim) : base(_perpetrator, _victim)
        {
            EvenTowers[0] = _perpetrator;
            EvenTowers[1] = _victim;
        }
    }
}

