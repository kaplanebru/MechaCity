using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TowerExternal
{
    public class ShieldCollection : BaseTowerRelatedCollection<Shield>
    {
        public ShieldCollection(Shield[] collection) : base(collection)
        {
        }

        public override void Subscribe()
        {
            Eventbus.TowerEvents.OnShieldActionTriggered += RevealShields;
            //BpEventbus.ActionEvents.OnBreakShieldActionTriggered += BreakSelectedShield;
        }

        private void BreakSelectedShield(int[] towerIDs)
        {
            foreach (var id in towerIDs)
            {
                var shield = Collection[id];
                shield.BreakShield();
            }
        }

        private void RevealShields(int towerID, int towerHeight)
        {
            var shield = Collection[towerID];
            shield.RevealShield(towerHeight);
        }


        public override void Unsubscribe()
        {
            Eventbus.TowerEvents.OnShieldActionTriggered -= RevealShields;
            //BpEventbus.ActionEvents.OnBreakShieldActionTriggered -= BreakSelectedShield;
        }
    }
}