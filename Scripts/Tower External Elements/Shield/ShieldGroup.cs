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
            BpEventbus.ActionEvents.OnBreakShieldActionTriggered += BreakSelectedShield;
        }

        private void BreakSelectedShield(int[] towerIDs)
        {
            foreach (var id in towerIDs)
            {
                var shield = Group[id];
                shield.BreakShield();
            }
        }

        private void RevealShields(Vector2Int[] towersAndHeight)
        {
            foreach (var item in towersAndHeight)
            {
                var shield = Group[item.x];
                shield.RevealShield(item.y);
            }
        }


        public void Unsubscribe()
        {
            BpEventbus.ActionEvents.OnShieldActionTriggered -= RevealShields;
            BpEventbus.ActionEvents.OnBreakShieldActionTriggered -= BreakSelectedShield;
        }


       
    }
}