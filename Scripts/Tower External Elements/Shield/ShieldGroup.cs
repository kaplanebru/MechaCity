using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class ShieldGroup: BaseTowerExternalGroup<Shield>
    {
        public ShieldGroup(Shield[] group) : base(group)
        {
        }
     
        public void Subscribe()
        {
            BpEventbus.ActionEvents.OnShieldActionTriggered += RevealShields;

        }

        private void RevealShields(int[] towerIDs)
        {
            foreach (var towerID in towerIDs)
            {
                var shield = Group[towerID];
                //shield.RevealShield(); //todo: tower height
            }
        }


        public void Unsubscribe()
        {
            BpEventbus.ActionEvents.OnShieldActionTriggered -= RevealShields;

        }


       
    }
}