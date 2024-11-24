using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class ShieldGroups
    {
        private Shield[] _group;

        public ShieldGroups(Shield[] group)
        {
            _group = group;
        }

        public void Subscribe()
        {
            BpEventbus.ActionEvents.OnShieldActionTriggered += RevealShields;

        }

        private void RevealShields(int[] towerIDs)
        {
            foreach (var towerID in towerIDs)
            {
                var selectedShield = _group.FirstOrDefault(s => s.Id == towerID);
                //selectedShield.RevealShield(); //todo: tower height
            }
        }


        public void Unsubscribe()
        {
            BpEventbus.ActionEvents.OnShieldActionTriggered -= RevealShields;

        }
    }
}