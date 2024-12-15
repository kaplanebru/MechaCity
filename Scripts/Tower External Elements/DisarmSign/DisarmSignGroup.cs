using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerExternal
{
    public class DisarmSignGroup : BaseTowerExternalGroup<DisarmSign>
    {
        public DisarmSignGroup(DisarmSign[] group) : base(group)
        {
        }


        public override void Subscribe()
        {
            Eventbus.TowerEvents.OnDisarmamentActionTriggered += RevealSign;
        }

        private void RevealSign(int towerID)
        {
            var sign = Group[towerID];
            sign.RevealSign();
        }

        private void HideSign(int towerID)
        {
            var sign = Group[towerID];
            sign.HideSign();
        }

        public override void Unsubscribe()
        {
            Eventbus.TowerEvents.OnDisarmamentActionTriggered -= RevealSign;
        }
    }

}
